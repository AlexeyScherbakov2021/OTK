using System.Windows.Markup;
using System;

namespace OTK.Infrastructure
{
    //public enum EnumStatus : int { Created = 1, CoordinateWork, Coordinated, ApprovWork,  Approved, Return, Refused, Closed, Waiting, None };
    //public enum EnumTypesStep : int { Coordinate = 1,  Approve, Review, Notify, Created };
    //public enum EnumAction : int { Send, Return, Refuse, Close};
    //public enum EnumCheckedStatus : int { CheckedNone = 0, CheckedProcess,  Checked };

    public enum EnumRoles : int { Пользователь = 0, Управление, Admin };
    public class UserRolesBinding : MarkupExtension
    {
        public Type EnumType { get; private set; }

        public UserRolesBinding(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
                throw new Exception("не тот тип");

            EnumType = enumType;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }

    }


}