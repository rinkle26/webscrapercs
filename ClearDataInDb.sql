
delete from url where id>285;
delete from property where id > 0;
delete from propertytype where id > 0;
delete from expenses where id > 0;
delete from expensetype where id > 0;
delete from review where id > 0;
delete from apartments where id > 0;
delete from ntpi where id > 0;
delete from school where id > 0;
delete from amenity where id > 0;
delete from amenitytype where id > 0;
update url set status=0 where id = 285;
