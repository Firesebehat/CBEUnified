namespace eTaxAPI.Model
{
    public class ChangePasswordModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public bool IsNewOrReset { get; set; }
    }
}