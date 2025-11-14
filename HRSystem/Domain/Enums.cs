using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HRSystem.Domain;

public class Enums
{
    public enum Role { Staff, Clerk, Supervisor, Manager, Admin }

    public enum Status { Draft, Confirmed, Deleted }

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