CREATE PROCEDURE GetVistors
AS
	SELECT COUNT(Pid) AS 'total',  (SELECT COUNT(Pid) from Visitors WHERE LastOnlineTime > GETDATE()) AS 'online' FROM Visitors
	RETURN;
GO

CREATE PROCEDURE GetVistorsInMonth
@startDate datetime2,
@endDate datetime2
AS
	SELECT Count(Pid) AS 'total' FROM Visitors WHERE LoginTime >= @startDate and LoginTime <= @endDate
	RETURN;
GO


CREATE PROCEDURE GetVistorsInDate
@date datetime2
AS
	SELECT Count(Pid) AS 'total' FROM Visitors WHERE Day(LoginTime) = Day(@date) and Month(LoginTime) = Month(@date) and YEAR(LoginTime) = YEAR(@date)
	RETURN;
GO


CREATE PROCEDURE GetBarChart
@startDay int,
@endDay int,
@month int,
@year int
AS
BEGIN
	DECLARE @ret varchar(max)
	While @startDay <= @endDay
	BEGIN
		DECLARE @count int
		SET @count = (SELECT COUNT(Pid) FROM Visitors WHERE DAY(LastOnlineTime) = @startDay and MONTH(LastOnlineTime) = @month and YEAR(LastOnlineTime) = @year)
		SET @ret = CONCAT(@ret, CONVERT(varchar(24),@count) + ',');
		SET @startDay = @startDay + 1;
	END
	SELECT @ret AS 'BarChart'
	RETURN;
END


CREATE INDEX ProductCateIndex
ON ProductCates([Order], Deleted, [Enabled], isLocked);

CREATE INDEX MultiProductCateIndex
ON MultiLang_ProductCates(ProductCatePid);

CREATE INDEX ProductTypeIndex
ON ProductTypes([Order], Deleted, [Enabled], isLocked);

CREATE INDEX MultiProductTypeIndex
ON MultiLang_ProductTypes(ProductTypePid);

CREATE INDEX ProductIndex
ON ProductDetails([Order], Deleted, [Enabled]);

CREATE INDEX NewsCateIndex
ON NewsCates([Order], Deleted, [Enabled], isLocked);

CREATE INDEX MultiNewsCateIndex
ON MultiLang_NewsCates(NewsCatePid);

CREATE INDEX NewsIndex
ON NewsDetails([Order], Deleted, [Enabled]);

CREATE INDEX Visitor_Index
ON Visitors (Id, LastOnlineTime, LoginTime);

CREATE INDEX ClientIndex
ON Clients([Order], Deleted, [Enabled]);

CREATE INDEX CommentIndex
ON Comments([Order], Deleted, [Enabled]);

CREATE INDEX CustomerIndex
ON Customers (Deleted, [Enabled]);
