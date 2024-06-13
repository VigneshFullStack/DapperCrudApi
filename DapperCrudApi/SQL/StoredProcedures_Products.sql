-- get all products
CREATE PROCEDURE GetProducts
AS
BEGIN
	SELECT * FROM Products
END
Go

-- get a single product
CREATE PROCEDURE GetProductById
	@Id	  INT
AS
BEGIN
	SELECT * FROM Products WHERE Id = @Id
END
Go

-- add a new product
CREATE PROCEDURE AddProduct
	@Name	NVARCHAR(100),
	@Price  DECIMAL(18, 2),
	@Stock  INT
AS 
BEGIN
	INSERT INTO Products (Name, Price, Stock)
	VALUES (@Name, @Price, @Stock)
	SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id
END
Go

-- update a product
CREATE PROCEDURE UpdateProduct
	@Id		INT,
	@Name	NVARCHAR(100),
	@Price  DECIMAL(18, 2),
	@Stock  INT
AS
BEGIN
	UPDATE Products
	SET Name  = @Name, 
		Price = @Price,
		Stock = @Stock
	WHERE Id  = @Id
END
Go

-- delete a product
CREATE PROCEDURE DeleteProduct
	@Id		INT
AS
BEGIN
	DELETE FROM Products
	WHERE Id = @Id
END
