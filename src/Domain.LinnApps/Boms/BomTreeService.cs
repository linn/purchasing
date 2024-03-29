﻿namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomTreeService : IBomTreeService
    {
        private readonly IRepository<Bom, int> repository;

        private readonly IBomDetailViewRepository detailViewRepository;

        public BomTreeService(
            IRepository<Bom, int> repository,
            IBomDetailViewRepository detailViewRepository)
        {
            this.repository = repository;
            this.detailViewRepository = detailViewRepository;
        }

        public BomTreeNode BuildBomTree(
            string bomName, 
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false)
        {
            // given the root node, we need to make repository method calls until this entire bom tree is populated with information

            // basically level order traversal of a general tree
            // where each time we visit a node we populate its children's children by performing an additional .FilterBy() lookup
            
            // look up the root node
            var root = this.repository.FindBy(x => x.BomName == bomName);
            if (root == null)
            {
                return null;
            }
            var rootNode = new BomTreeNode
                               {
                                   Name = root.BomName,
                                   Id = "root",
                                   Type = root.Part?.BomType,
                                   Description = root.Part?.Description,
                                   SafetyCritical = root.Part?.SafetyCritical,
                                   Children = root.Details
                                       .Where(x => showChanges || x.ChangeState == "LIVE")
                                       .Where(c => !requirementOnly 
                                                   || c.PartRequirement is { AnnualUsage: > 0 })
                                       ?.Select(
                                       d => new BomTreeNode
                                                {
                                                    Name = d.Part.PartNumber,
                                                    Description = d.Part.Description,
                                                    ParentName = d.BomName,
                                                    ParentId = d.BomId.ToString(),
                                                    Qty = d.Qty,
                                                    ChangeState = d.ChangeState,
                                                    IsReplaced = d.DeleteChangeId.HasValue,
                                                    Requirement = d.GenerateRequirement,
                                                    DrawingReference = d.Part.DrawingReference,
                                                    SafetyCritical = d.Part.SafetyCritical,
                                                    Type = d.Part.BomType,
                                                    Id = d.DetailId.ToString(),
                                                    AddChangeDocumentNumber = d.AddChange.DocumentNumber,
                                                    DeleteChangeDocumentNumber = d.DeleteChange?.DocumentNumber,
                                                    AddReplaceSeq = d.AddReplaceSeq,
                                                    DeleteReplaceSeq = d.DeleteReplaceSeq
                                                }).OrderBy(x => x.Name)
                               };

            // keep track of the depth so we can optionally exit out if the specified levels depth is met
            var currentDepth = 1;

            // queue the root node
            var q = new Queue<BomTreeNode>();
            q.Enqueue(rootNode);

            // while there are nodes in the queue
            while (q.Count != 0)
            {
                if (currentDepth == levels)
                {
                    break;
                }

                // while this node has children
                var numChildren = q.Count;
                while (numChildren > 0)
                {
                    // dequeue a child node
                    var current = q.Dequeue();

                    // populate it's children's children nodes by performing a repository lookup
                    current.Children = current.Children?.Select(
                        child =>
                            {
                                var children = child.Type != "C" ? this.detailViewRepository
                                                       .FilterBy(x => x.BomName == child.Name)
                                                       .Where(x => showChanges || x.ChangeState == "LIVE")
                                                       .Where(c => !requirementOnly
                                                                   || (c.PartRequirement != null && c.PartRequirement.AnnualUsage > 0))
                                                   :null;

                                var node = new BomTreeNode
                                {
                                    Name = child.Name,
                                    Description = child.Description,
                                    Qty = child.Qty,
                                    Id = child.Id,
                                    SafetyCritical = child.SafetyCritical,
                                    Type = child.Type,
                                    ParentName = current.Name,
                                    ParentId = current.Id,
                                    ChangeState = child.ChangeState,
                                    Requirement = child.Requirement,
                                    DrawingReference = child.DrawingReference,
                                    AddChangeDocumentNumber = child.AddChangeDocumentNumber,
                                    DeleteChangeDocumentNumber = child.DeleteChangeDocumentNumber,
                                    IsReplaced = child.IsReplaced,
                                    AddReplaceSeq = child.AddReplaceSeq,
                                    DeleteReplaceSeq = child.DeleteReplaceSeq,
                                    Children =
                                    children?
                                        .OrderBy(x => x.Part.PartNumber)
                                        .Select(
                                            detail =>
                                                new BomTreeNode
                                                    {
                                                        Name = detail.Part.PartNumber,
                                                        Description = detail.Part.Description,
                                                        Qty = detail.Qty,
                                                        Type = detail.Part.BomType,
                                                        ParentName = detail.BomPartNumber,
                                                        ParentId = detail.BomId.ToString(),
                                                        Requirement = detail.GenerateRequirement,
                                                        SafetyCritical = detail.Part.SafetyCritical,
                                                        DrawingReference = detail.Part.DrawingReference,
                                                        AddChangeDocumentNumber = detail.AddChange.DocumentNumber,
                                                        DeleteChangeDocumentNumber = detail.DeleteChange != null ?
                                                            detail.DeleteChange.DocumentNumber : null,
                                                        ChangeState = detail.ChangeState,
                                                        IsReplaced = detail.DeleteChangeId.HasValue,
                                                        Id = detail.DetailId.ToString(),
                                                        AddReplaceSeq = detail.AddReplaceSeq,
                                                        DeleteReplaceSeq = detail.DeleteReplaceSeq
                                                    })
                                };
                                return node;
                        }).ToList();

                    // add all the children (now with their own children populated) to the queue, if they exist
                    if (current.Children != null)
                    {
                        foreach (var child in current.Children)
                        {
                            q.Enqueue(child);
                        }
                    }

                    numChildren--;
                }

                currentDepth++;
            }

            // return the root node - all of its children will have children now, and so on...
            return rootNode;
        }

        public IEnumerable<BomTreeNode> FlattenBomTree(
            string bomName, 
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false)
        {
            // same as above except each time we visit a node we add it to the results list
            var result = new List<BomTreeNode>();
            var root = this.repository.FindBy(x => x.BomName == bomName);
            if (root == null)
            {
                return result;
            }
            var rootNode = new BomTreeNode
            {
                Name = root.BomName,
                Children = root.Details
                    .Where(c => !requirementOnly
                                || c.PartRequirement is { AnnualUsage: > 0 })
                    .Where(x => showChanges || x.ChangeState == "LIVE")
                    ?.Select(
                        d => new BomTreeNode
                                 {
                                     Name = d.Part.PartNumber,
                                     Description = d.Part.Description,
                                     Cost = d.Part.ExpectedUnitPrice,
                                     Qty = d.Qty,
                                     AddChangeId = d.AddChangeId,
                                     DeleteChangeId = d.DeleteChangeId,
                                     Type = d.Part.BomType,
                                     ParentName = root.BomName,
                                     Id = d.DetailId.ToString(),
                                 }).OrderBy(x => x.Name)
            };
            var q = new Queue<BomTreeNode>();
            q.Enqueue(rootNode);
            var currentDepth = 0;

            while (q.Count != 0)
            {
                if (currentDepth > levels && levels != 0)
                {
                    break;
                }

                var numChildren = q.Count;

                while (numChildren > 0)
                {
                    var current = q.Dequeue();
                    result.Add(current);
                    current.Children = current.Children?.Select(
                        child =>
                        {
                            var children = child.Type != "C" ? this.detailViewRepository
                                .FilterBy(x => x.BomName == child.Name)
                                .Where(x => showChanges || x.ChangeState == "LIVE")
                                .Where(c => !requirementOnly
                                            || (c.PartRequirement != null && c.PartRequirement.AnnualUsage > 0))
                                               : null;
                            var node = new BomTreeNode
                            {
                                Name = child.Name,
                                Description = child.Description,
                                Qty = child.Qty,
                                ParentName = child.ParentName,
                                Id = child.Id,
                                Cost = child.Cost,
                                Type = child.Type,
                                AddChangeId = child.AddChangeId,
                                DeleteChangeId = child.DeleteChangeId,
                                ParentId = current.Id,
                                Children = children?
                                        .OrderBy(x => x.Part.PartNumber)
                                        .Select(
                                            detail =>
                                                new BomTreeNode
                                                {
                                                    Name = detail.Part.PartNumber,
                                                    Description = detail.Part.Description,
                                                    Qty = detail.Qty,
                                                    Type = detail.Part.BomType,
                                                    ParentName = child.Name,
                                                    AddChangeId = detail.AddChangeId,
                                                    DeleteChangeId = detail.DeleteChangeId,
                                                    Cost = detail.Part.ExpectedUnitPrice,
                                                    ParentId = child.Id,
                                                    Id = detail.DetailId.ToString()
                                                })
                            };
                            return node;
                        }).ToList();
                    if (current.Children != null)
                    {
                        foreach (var child in current.Children)
                        {
                            q.Enqueue(child);
                        }
                    }

                    numChildren--;
                }

                currentDepth++;
            }

            return result;
        }

        public BomTreeNode BuildWhereUsedTree(
            string partNumber,
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false)
        {
            var rootNode = new BomTreeNode
                               {
                                   Name = partNumber,
                                   Qty = 0,
                                   Id = "-1",
                                   Children = this.detailViewRepository.FilterBy(d => d.PartNumber == partNumber)
                                       .Where(c => !requirementOnly
                                                   || (c.BomPart.PartRequirement != null && c.BomPart.PartRequirement.AnnualUsage > 0))
                                       .Where(x => showChanges || x.ChangeState == "LIVE")
                                       .Select(c => new BomTreeNode
                                                        {
                                                            Name = c.BomPart.PartNumber,
                                                            Description = c.BomPart.Description,
                                                            Qty = c.Qty,
                                                            Id = c.DetailId.ToString(),
                                                            DeleteChangeDocumentNumber = c.DeleteChange != null ?
                                                                c.DeleteChange.DocumentNumber : null,
                                                            AddReplaceSeq = c.AddReplaceSeq,
                                                            DeleteReplaceSeq = c.DeleteReplaceSeq,
                                                            ChangeState = c.ChangeState,
                                                            IsReplaced = c.DeleteChangeId.HasValue,
                                                            PcasLine = c.PcasLine
                                       }).OrderBy(c => c.Name)

                               };
            var currentDepth = 1;
            var q = new Queue<BomTreeNode>();
            q.Enqueue(rootNode);
            while (q.Count != 0)
            {
                if (currentDepth == levels && levels != 0)
                {
                    break;
                }

                var numChildren = q.Count;
                while (numChildren > 0)
                {
                    var current = q.Dequeue();
                        current.Children = current.Children?.Select(
                        child =>
                        {
                            var children = this.detailViewRepository
                                .FilterBy(x => x.PartNumber == child.Name)
                                .Where(x => showChanges || x.ChangeState == "LIVE")
                                .Where(c => !requirementOnly
                                            || (c.BomPart.PartRequirement != null && c.BomPart.PartRequirement.AnnualUsage > 0));

                            var node = new BomTreeNode
                            {
                                Name = child.Name,
                                Description = child.Description,
                                Qty = child.Qty,
                                Type = child.Type,
                                Id = child.Id,
                                DeleteChangeDocumentNumber = child.DeleteChangeDocumentNumber,
                                AddReplaceSeq = child.AddReplaceSeq,
                                DeleteReplaceSeq = child.DeleteReplaceSeq,
                                ChangeState = child.ChangeState,
                                IsReplaced = child.IsReplaced,
                                PcasLine = child.PcasLine,
                                Children =
                                children?
                                    .Select(
                                        detail =>

                                            new BomTreeNode
                                            {
                                                Name = detail.BomPart.PartNumber,
                                                Description = detail.BomPart.Description,
                                                Qty = detail.Qty,
                                                Id = detail.DetailId.ToString(),
                                                DeleteChangeDocumentNumber = detail.DeleteChange != null ?
                                                                                 detail.DeleteChange.DocumentNumber : null,
                                                AddReplaceSeq = detail.AddReplaceSeq,
                                                DeleteReplaceSeq = detail.DeleteReplaceSeq,
                                                ChangeState = detail.ChangeState,
                                                IsReplaced = detail.DeleteChangeId.HasValue,
                                                PcasLine = detail.PcasLine
                                            })
                                    .OrderBy(c => c.Name)
                            };
                            return node;
                        }).ToList();

                    if (current.Children != null)
                    {
                        foreach (var child in current.Children)
                        {
                            q.Enqueue(child);
                        }
                    }

                    numChildren--;
                }

                currentDepth++;
            }
            return rootNode;
        }

        public IEnumerable<BomTreeNode> FlattenWhereUsedTree(
            string partNumber,
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false)
        {
            var result = new List<BomTreeNode>();
            var rootNode = new BomTreeNode
            {
                Name = partNumber,
                Qty = 0,
                Children = this.detailViewRepository.FilterBy(d => d.PartNumber == partNumber)
                    .Where(c => !requirementOnly
                                || (c.BomPart.PartRequirement != null && c.BomPart.PartRequirement.AnnualUsage > 0))
                    .Where(x => showChanges || x.ChangeState == "LIVE")
                                       .Select(c => new BomTreeNode
                                       {
                                           Name = c.BomPart.PartNumber,
                                           Description = c.BomPart.Description,
                                           Qty = c.Qty,
                                           DeleteChangeDocumentNumber = c.DeleteChange != null ?
                                                                            c.DeleteChange.DocumentNumber : null,
                                           AddReplaceSeq = c.AddReplaceSeq,
                                           DeleteReplaceSeq = c.DeleteReplaceSeq,
                                           ChangeState = c.ChangeState,
                                           IsReplaced = c.DeleteChangeId.HasValue,
                                           PcasLine = c.PcasLine
                                       }).OrderBy(c => c.Name)

            };
            var currentDepth = 1;
            var q = new Queue<BomTreeNode>();
            q.Enqueue(rootNode);
            while (q.Count != 0)
            {
                if (currentDepth == levels)
                {
                    break;
                }

                var numChildren = q.Count;
                while (numChildren > 0)
                {
                    var current = q.Dequeue();
                    result.Add(current);
                    current.Children = current.Children?.Select(
                        child =>
                        {
                            var children = this.detailViewRepository
                                .FilterBy(x => x.PartNumber == child.Name)
                                .Where(x => showChanges || x.ChangeState == "LIVE")
                                .Where(c => !requirementOnly
                                            || (c.BomPart.PartRequirement != null && c.BomPart.PartRequirement.AnnualUsage > 0));

                            var node = new BomTreeNode
                            {
                                Name = child.Name,
                                Description = child.Description,
                                Qty = child.Qty,
                                DeleteChangeDocumentNumber = child.DeleteChangeDocumentNumber,
                                AddReplaceSeq = child.AddReplaceSeq,
                                DeleteReplaceSeq = child.DeleteReplaceSeq,
                                ChangeState = child.ChangeState,
                                IsReplaced = child.IsReplaced,
                                PcasLine = child.PcasLine,
                                Children =
                                children
                                    .Select(
                                        detail =>

                                            new BomTreeNode
                                            {
                                                Name = detail.BomPart.PartNumber,
                                                Description = detail.BomPart.Description,
                                                Qty = detail.Qty,
                                                DeleteChangeDocumentNumber = detail.DeleteChange != null ?
                                                                                 detail.DeleteChange.DocumentNumber : null,
                                                AddReplaceSeq = detail.AddReplaceSeq,
                                                DeleteReplaceSeq = detail.DeleteReplaceSeq,
                                                ChangeState = detail.ChangeState,
                                                IsReplaced = detail.DeleteChangeId.HasValue,
                                                PcasLine = detail.PcasLine
                                            })
                                    .OrderBy(c => c.Name)
                            };
                            return node;
                        }).ToList();

                    if (current.Children != null)
                    {
                        foreach (var child in current.Children)
                        {
                            q.Enqueue(child);
                        }
                    }

                    numChildren--;
                }

                currentDepth++;
            }

            return result;
        }
    }
}
