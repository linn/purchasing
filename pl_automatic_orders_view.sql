create or replace view pl_automatic_orders_view as 
select
m.part_number, m.recom_purch_order_qty,
m.recom_purch_order_date mr_recom_purch_order_date,
decode(
  supp_pack.delivery_day(m.preferred_supplier), 
  rtrim(to_char(m.recom_purch_order_date, 'DAY')),
  m.recom_purch_order_date,
  next_day(m.recom_purch_order_date, supp_pack.delivery_day(m.preferred_supplier))
  ) recom_purch_order_date,
m.recom_purch_order_code, ps.curr_code,
ps.currency_unit_price_ours,
m.preferred_supplier, s.supplier_name,
ps.pl_order_method, ps.jit_reorder_order_number,
s.vendor_manager, s.planner,
m.jobref
from v_master_mrh m, suppliers s, part_suppliers ps
where m.recom_purch_order_code in ('POLICY', 'CHECK1', 'CHECK2')
and m.preferred_supplier = s.supplier_id
and m.part_number = ps.part_number
and m.preferred_supplier = ps.supplier_id
order by m.preferred_supplier, m.part_number
/