CREATE OR ALTER PROCEDURE dbo.USP_GetRecepit
(
	@PaymentID INT
)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 
	SET QUOTED_IDENTIFIER OFF      
	SET NOCOUNT ON    
	
	SELECT M.MemberNo,
		 ISNULL(M.MemberFName,'')
		 +	(CASE WHEN ISNULL(M.MemberLName,'') ='' THEN '' ELSE ' ' END)
		 +	ISNULL(M.MemberLName,'') + 
		 (CASE WHEN ISNULL(M.MemberMName,'') ='' THEN '' ELSE ' ' END )+ISNULL(M.MemberMName,'')  AS MemberName,
		 [PLAN].PlanName,
		 SM.SchemeName,
		 P.PaymentFromdt,
		 P.PaymentTodt,
		 P.NextRenwalDate,
		 P.PaymentAmount,
		 P.CreateDate,
		 [PLAN].ServiceTax,
		 [PLAN].PlanAmount,
		 [PLAN].ServicetaxAmount 
	FROM MemberRegistration M WITH(NOLOCK)
		INNER JOIN PaymentDetails P WITH(NOLOCK) ON P.MemberId= M.MemberId
		INNER JOIN PlanMaster [PLAN] WITH(NOLOCK) ON [PLAN].PlanID= M.PLANID
		INNER JOIN SchemeMaster SM WITH(NOLOCK) ON SM.SchemeID= M.SchemeID
	WHERE P.PaymentID= @PaymentID
	SET QUOTED_IDENTIFIER ON      
	SET NOCOUNT OFF   
END
 
