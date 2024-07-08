export class PaymentDetailsModel 
{
    public PaymentID: number=0;
    public PlanName: string="";
    public SchemeName: string="";
    public PaymentAmount: number=0;
    public PaymentFromdt: Date=new Date();
    public PaymentTodt: Date=new Date();
    public NextRenwalDate: Date=new Date();
    public RecStatus: string="";
    public MemberName: string="";
    public MemberNo: string="";
}