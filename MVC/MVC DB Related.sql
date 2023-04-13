-- Create table
USE [Movies]
GO

/****** Object:  Table [dbo].[Movie_Details]    Script Date: 13-04-2023 12:26:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Movie_Details](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](30) NOT NULL,
	[rating] [float] NULL,
	[release_year] [int] NOT NULL,
	[director] [nchar](30) NULL,
	[Genre] [nchar](30) NULL
) ON [PRIMARY]
GO

-- Insert values
insert into Movie_Details values('Interstellar',8.6,2014,'Christopher Nolan','Sci-Fi');

select * from Movie_Details;

-- Add Stored Procedures
create procedure addMovies @name nchar(30),@rating float,@release_year int,@director nchar(30),@genre nchar(30) as
insert into Movie_Details values(@name,@rating,@release_year,@director,@genre);

create or alter procedure editMovies @id int,@name nchar(30),@rating float,@release_year int,@director nchar(30),@genre nchar(30) as
update Movie_Details set name=@name,rating=@rating,release_year=@release_year,director=@director,genre=@genre
where id=@id;

create or alter procedure fetch_movieDetails @id int as
select * from Movie_Details where id=@id;

create or alter procedure deleteMovies @id int as
delete from Movie_Details where id=@id;