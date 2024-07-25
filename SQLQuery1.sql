 CREATE TABLE users
 (
   id INT PRIMARY KEY IDENTITY(1,1),
   username VARCHAR(MAX) NULL,
   password VARCHAR(MAX) NULL
 
 )

 SELECT * FROM users
 
 INSERT  users (username, password) VALUES('admin', 'admin123')