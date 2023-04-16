Create table users(
		userId int not null primary key,
		name nchar(50) not null,
		emailId nchar(50) not null,
		password nchar(50) not null,
		phnumber int
		);
Create table appointments(
		id int primary key,
		startTime datetime not null,
		endTime datetime,
		createdTime datetime,
		updatedTime datetime,
		meet_description varchar(255) null,
		meet_status nchar(20) not null,
		place nchar(20) not null,
		meeting_person nchar(20) not null,
		purpose nchar(20) null,
		reference nchar(50) null 
		foreign key (id) references users(userId) on delete cascade,
		);
-- Get User
create or alter procedure getUser @userid int
as
select * from users where userId = @userid;

-- Insert User
create or alter procedure insertUser 
@userid int,@username nchar(50),@useremail nchar(50),@userpassword nchar(50),@userphonenumber int
as
insert into users values (@userid,@username,@useremail,@userpassword,@userphonenumber);



