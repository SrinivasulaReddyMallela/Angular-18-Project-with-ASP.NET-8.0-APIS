CREATE OR ALTER PROCEDURE dbo.Usp_GetMonthwisepaymentdetails
(
	@month	VARCHAR(MAX)
)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 
	SET QUOTED_IDENTIFIER OFF      
	SET NOCOUNT ON 

	SELECT MemberFName,M.MemberNo,MemberLName,MemberMName, JOININGDATE AS CreateDate,
		PD.PaymentAmount AS Total,MONTH(PaymentFromdt) AS Paymentmonth,PD.PaymentAmount,NULL Username
	FROM MemberRegistration M WITH(NOLOCK) 
	INNER JOIN PaymentDetails PD WITH (NOLOCK) ON PD.MemberID= M.MemberId
	WHERE MONTH(PaymentFromdt)=@month

	SET QUOTED_IDENTIFIER ON      
	SET NOCOUNT OFF
END
 

 
 
 
 
 
 
