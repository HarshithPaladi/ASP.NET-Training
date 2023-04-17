create table webEditor (
doc_id int primary key,
doc_title nvarchar(500),
doc_data NVARCHAR(MAX),
date_created datetime,
date_updated datetime null,
author nchar(255),
);

create or alter procedure insertData
@doc_id int,@doc_title nvarchar(500), @doc_data NVARCHAR(MAX), @date_created datetime,
@date_updated datetime, @author nchar(255)
as
insert into webEditor values(@doc_id,@doc_title,@doc_data,@date_created,@date_updated,@author);

create or alter procedure fetch_docDetails
@doc_id int
as
select * from webEditor where doc_id = @doc_id;

create or alter procedure editDoc
@doc_id int, @doc_data NVARCHAR(MAX),@doc_title nvarchar(500),
@date_updated datetime, @author nchar(255)
as
update webEditor set doc_data = @doc_data,doc_title = @doc_title,date_updated=@date_updated,author=@author 
where doc_id=@doc_id;