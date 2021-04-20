create procedure CreateUser(in usr varchar(50), in pwd varchar(64))
begin
	insert into users
	values (usr, pwd, now());
end ;