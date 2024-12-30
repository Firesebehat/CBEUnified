using System.Web.Profile;

namespace CProfile
{
    public class ProfileGroupOrganization : ProfileGroupBase
    {
        public virtual string Name
        {
            get { return (string) GetPropertyValue("Name"); }
            set { SetPropertyValue("Name", value); }
        }

        public virtual string GUID
        {
            get { return (string) GetPropertyValue("GUID"); }
            set { SetPropertyValue("GUID", value); }
        }

        public virtual string Administration
        {
            get { return (string) GetPropertyValue("Administration"); }
            set { SetPropertyValue("Administration", value); }
        }

        public virtual string Category
        {
            get { return (string) GetPropertyValue("Category"); }
            set { SetPropertyValue("Category", value); }
        }

        public virtual string Language
        {
            get { return (string) GetPropertyValue("Language"); }
            set { SetPropertyValue("Language", value); }
        }

        public virtual string Zone
        {
            get { return (string) GetPropertyValue("Zone"); }
            set { SetPropertyValue("Zone", value); }
        }

        public virtual string Wereda
        {
            get { return (string) GetPropertyValue("Wereda"); }
            set { SetPropertyValue("Wereda", value); }
        }
    }

    public class ProfileGroupStaff : ProfileGroupBase
    {
        public virtual string FullName
        {
            get { return (string) GetPropertyValue("FullName"); }
            set { SetPropertyValue("FullName", value); }
        }

        public virtual string GUID
        {
            get { return (string) GetPropertyValue("GUID"); }
            set { SetPropertyValue("GUID", value); }
        }

        public virtual string Units
        {
            get { return (string) GetPropertyValue("Units"); }
            set { SetPropertyValue("Units", value); }
        }
    }

    public class ProfileCommon : ProfileBase
    {
        public virtual ProfileGroupOrganization Organization
        {
            get { return (ProfileGroupOrganization) GetProfileGroup("Organization"); }
        }

        public virtual ProfileGroupStaff Staff
        {
            get { return (ProfileGroupStaff) GetProfileGroup("Staff"); }
        }

        public virtual ProfileCommon GetProfile(string username)
        {
            return (ProfileCommon) Create(username);
        }
    }
}