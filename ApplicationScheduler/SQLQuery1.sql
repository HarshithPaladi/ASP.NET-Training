Create table users(
		userId int not null primary key,
		userName nchar(50) not null,
		emailId nchar(50) not null,
		userPassword nchar(50) not null,
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

-- Insert Appointments
create or alter procedure addAppointment
@userid int,@starttime datetime,@endtime datetime, @createdtime datetime, @updatedtime datetime,
@desc varchar(255), @status nchar(20),@place nchar(20), @meet_person nchar(20),
@purpose nchar(20), @reference nchar(50)
as
insert into appointments values (
@userid,@starttime,@endtime,@createdtime,
@updatedtime,@desc,@status,@place,@meet_person,@purpose,@reference
);

