﻿namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public class CircuitBoardResourceBuilder : IBuilder<CircuitBoard>
    {
        public CircuitBoardResource Build(CircuitBoard entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new CircuitBoardResource { Links = this.BuildLinks(null, claims).ToArray() };
            }

            return new CircuitBoardResource
                       {
                           BoardCode = entity.BoardCode,
                           Description = entity.Description,
                           ChangeId = entity.ChangeId,
                           ChangeState = entity.ChangeState,
                           SplitBom = entity.SplitBom,
                           DefaultPcbNumber = entity.DefaultPcbNumber,
                           VariantOfBoardCode = entity.VariantOfBoardCode,
                           LoadDirectory = entity.LoadDirectory,
                           BoardsPerSheet = entity.BoardsPerSheet,
                           CoreBoard = entity.CoreBoard,
                           ClusterBoard = entity.ClusterBoard,
                           IdBoard = entity.IdBoard,
                           Layouts = entity.Layouts?.OrderBy(a => a.LayoutSequence).Select(MakeLayoutResource),
                           Links = this.BuildLinks(entity, claims).ToArray()
                       };
        }

        public string GetLocation(CircuitBoard entity)
        {
            return $"/purchasing/boms/boards/{entity.BoardCode}";
        }

        object IBuilder<CircuitBoard>.Build(CircuitBoard entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private static BoardLayoutResource MakeLayoutResource(BoardLayout a)
        {
            return new BoardLayoutResource
                       {
                           BoardCode = a.BoardCode,
                           LayoutCode = a.LayoutCode,
                           LayoutSequence = a.LayoutSequence,
                           PcbNumber = a.PcbNumber,
                           LayoutType = a.LayoutType,
                           LayoutNumber = a.LayoutNumber,
                           PcbPartNumber = a.PcbPartNumber,
                           ChangeId = a.ChangeId,
                           ChangeState = a.ChangeState
                       };
        }

        private IEnumerable<LinkResource> BuildLinks(CircuitBoard model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
