using System;

namespace ch.thommenmedia.common.Interfaces
{

    public interface IPrivilegeBase
    {
        string Name { get; set; }
        Guid Id { get; set; }
    }

    public interface IPrivilege : IPrivilegeBase
    {
        Guid GroupId { get; set; }

        Guid PrivilegeId { get; set; }
        Guid? AccessRightId { get; set; }
        Guid UserId { get; set; }
        Guid? ObjectTypeId { get; set; }

        string GroupName { get; set; }
        string GroupDescription { get; set; }
        string UserName { get; set; }

        bool UserLocked { get; set; }

        string AccessRightName { get; set; }
        string PrivilegeName { get; set; }
        string ObjectName { get; set; }
        string TableName { get; set; }

        string PrivilegeDescription { get; set; }

        string Text { get; }
    }
}
