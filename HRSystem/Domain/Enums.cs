using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HRSystem.Domain;

public class Enums
{

    public enum Status
    {
        [Display(Name = "Contract Draft")]
        ContractDraft,
        [Display(Name = "Contract Completed")]
        ContractCompleted,
        [Display(Name = "Payroll Completed")]
        PayrollCompleted,
        Terminated
    }

    public enum Position
    {
        Clerk,
        Teacher,
        Principal,
        [Display(Name = "HR Assistant")]
        HRAssistant
    }

    public enum Dialog
    {
        primary,
        secondary,
        success,
        danger,
        warning,
        info,
        light,
        dark
    }
}