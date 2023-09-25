using Back_end.Services;

namespace Back_end.Models
{
    public enum Type
    {
        [EnumDisplayName("Vog")]
        Vog,
        [EnumDisplayName("Contract")]
        Contract,
        [EnumDisplayName("Paspoort")]
        Paspoort,
        [EnumDisplayName("Id_kaart")]
        Id_kaart,
        [EnumDisplayName("Diploma")]
        Diploma,
        [EnumDisplayName("Certificaat")]
        Certificaat,
        [EnumDisplayName("Lease_auto")]
        Lease_auto
    }
}