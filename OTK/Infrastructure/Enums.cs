using System.Windows.Markup;
using System;
using System.Windows.Documents;
using System.Collections.Generic;

namespace OTK.Infrastructure
{
    //public enum EnumStatus : int { Created = 1, CoordinateWork, Coordinated, ApprovWork,  Approved, Return, Refused, Closed, Waiting, None };
    
    public enum EnumFilter : int { Require, Works, Closed };


    public enum EnumFormType : int { VK = 0, OK, PSI, RN, Fail = -1 };

    public enum EnumStatusJob : int { 
        InWork,         // в работе
        ReqConfirm,     // требующий рассмотрения
        Complete,       // выполнен
        Closed          // в архиве (закрыт)
    };

    public static class ClassStatusJob
    {
        public static List<string> NameStatus { get; } = new List<string>()
        {
            "в работе", "проверка", "выполнен", "архив"
        };
    }



    public enum EnumStatus : int { 
        CheckedProcess = 0,     // в ожидании выполнения
        Checked,                // предварительно выполнен
        Finish,                 // подтверждено выполнение
        CheckedNone             // не выполнен в срок
    };

    public static class ClassStatus
    {
        public static List<string> NameStatus { get; } = new List<string>()
        {
            "В работе", "На проверке", "Выполнен", "Не выполнен"
        };
    }



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