create table webEditor (
doc_id int primary key,
doc_data varchar(max),
date_created datetime,
date_updated datetime null,
author nchar(255),
);

create or alter procedure insertData
@doc_id int, @doc_data varchar(max), @date_created datetime,
@date_updated datetime, @author nchar(255)
as
insert into webEditor values(@doc_id,@doc_data,@date_created,@date_updated,@author);

create or alter procedure fetch_docDetails
@doc_id int
as
select * from webEditor where doc_id = @doc_id;