CREATE OR ALTER PROCEDURE dbo.Usp_GetYearwisePAYMENTDETAILS
(
	@year	VARCHAR(MAX)
)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 
	SET QUOTED_IDENTIFIER OFF      
	SET NOCOUNT ON 
	CREATE TABLE #TEMP
	(
		CurrentYear INT,
		Jan			INT, 
		Feb			INT, 
		[March]		INT, 
		[April]		INT, 
		[May]		INT, 
		[June]		INT,
		[July]		INT, 
		[August]	INT, 
		Sept		INT, 
		Oct			INT, 
		Nov			INT, 
		Decm		INT,
		Total		INT
	)

	DECLARE  @PYEAR INT 
	DECLARE PAYMENTDETAIL_Cursor CURSOR LOCAL FAST_FORWARD FOR  
	SELECT YEAR(PaymentFromdt) AS [YEAR] FROM PAYMENTDETAILS WITH (NOLOCK)
	OPEN PAYMENTDETAIL_Cursor  	  
	FETCH NEXT FROM PAYMENTDETAIL_Cursor 
	INTO @PYEAR
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		INSERT INTO #TEMP(CurrentYear,Jan,Feb,[March],[April],[May],[June],[July],[August],Sept,Oct,Nov,Decm)
		SELECT  @year CurrentYear,  [January] AS Jan, [February] AS Feb, [March], [April], [May], [June],
				[July], [August], [September] AS Sept, [October] AS Oct, [November] AS Nov, [December] AS Decm
		FROM (
				SELECT DATENAME(MONTH, PaymentFromdt) AS [Month],
					SUM(PaymentAmount) AS Total
				FROM PAYMENTDETAILS WHERE YEAR(PaymentFromdt) =@PYEAR 
				GROUP BY DATENAME(MONTH, PaymentFromdt)
		) AS SourceData
		PIVOT (
			SUM(Total)
			FOR [Month] IN (
				[January], [February], [March], [April], [May], [June],
				[July], [August], [September], [October], [November], [December]
			)
		) AS PivotTable;

		IF EXISTS (SELECT 1 FROM #TEMP WHERE CurrentYear= @PYEAR)
		BEGIN
			UPDATE #TEMP 
				SET Total =(SELECT SUM(PaymentAmount) AS Total FROM PAYMENTDETAILS WHERE YEAR(PaymentFromdt) =@PYEAR GROUP BY DATENAME(MONTH, PaymentFromdt))
			WHERE CurrentYear = @PYEAR
		END
	FETCH NEXT FROM PAYMENTDETAIL_Cursor 
	INTO @PYEAR
	END  
	CLOSE PAYMENTDETAIL_Cursor  
	DEALLOCATE PAYMENTDETAIL_Cursor 
	SELECT * FROM #TEMP WHERE CurrentYear = CAST(@year AS int)
	DROP TABLE IF EXISTS #TEMP
	SET QUOTED_IDENTIFIER ON      
	SET NOCOUNT OFF
END
 