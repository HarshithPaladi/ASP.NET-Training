create procedure insertProducts
@id nchar(20),
@name nchar(100),
@sold int,
@avail int,
@price float
as
insert into Products values(@id,@name,@price,@avail,@sold);

--
create procedure Edit_Prod
@id nchar(20),
@name nchar(100),
@sold int,
@avail int,
@price float
as
update Products set product_name=@name,price=@price,quantity_remaining=@avail,quantity_sold=@sold where product_code=@id;

--
create procedure delete_prod
@id nchar(20)
as 
delete Products where product_code=@id;
exec delete_prod 'test';