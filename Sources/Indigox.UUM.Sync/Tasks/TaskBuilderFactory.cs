using System;
using System.Collections.Generic;
using Indigox.Common.EventBus.Interface.Event;
using Indigox.Common.Membership.Events;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Tasks.Builders;

namespace Indigox.UUM.Sync.Tasks
{
    internal class TaskBuilderFactory
    {
        private static TaskBuilderFactory instance = new TaskBuilderFactory();

        private TaskBuilderFactory()
        {
        }

        public static TaskBuilderFactory Instance
        {
            get { return instance; }
        }

        // <EventType, TaskBuilderType>
        private static Dictionary<Type, Type> mapping = new Dictionary<Type, Type>() {
            { typeof(GroupCreatedEvent                                    ), typeof(GroupCreatedEventTaskBuilder                                    ) },
            { typeof(GroupDeletedEvent                                    ), typeof(GroupDeletedEventTaskBuilder                                    ) },
            { typeof(GroupPropertyChangedEvent                            ), typeof(GroupPropertyChangedEventTaskBuilder                            ) },
            { typeof(OrganizationalRoleAddedToGroupEvent                  ), typeof(OrganizationalRoleAddedToGroupEventTaskBuilder                  ) },
            { typeof(OrganizationalRoleAddedToOrganizationalUnitEvent     ), typeof(OrganizationalRoleAddedToOrganizationalUnitEventTaskBuilder     ) },
            { typeof(OrganizationalRoleAddedToRoleEvent                   ), typeof(OrganizationalRoleAddedToRoleEventTaskBuilder                   ) },
            { typeof(OrganizationalRoleCreatedEvent                       ), typeof(OrganizationalRoleCreatedEventTaskBuilder                       ) },
            { typeof(OrganizationalRoleDeletedEvent                       ), typeof(OrganizationalRoleDeletedEventTaskBuilder                       ) },
            { typeof(OrganizationalRolePropertyChangedEvent               ), typeof(OrganizationalRolePropertyChangedEventTaskBuilder               ) },
            { typeof(OrganizationalRoleRemovedFromGroupEvent              ), typeof(OrganizationalRoleRemovedFromGroupEventTaskBuilder              ) },
            { typeof(OrganizationalRoleRemovedFromOrganizationalUnitEvent ), typeof(OrganizationalRoleRemovedFromOrganizationalUnitEventTaskBuilder ) },
            { typeof(OrganizationalRoleRemovedFromRoleEvent               ), typeof(OrganizationalRoleRemovedFromRoleEventTaskBuilder               ) },
            { typeof(OrganizationalUnitAddedToGroupEvent                  ), typeof(OrganizationalUnitAddedToGroupEventTaskBuilder                  ) },
            { typeof(OrganizationalUnitAddedToOrganizationalUnitEvent     ), typeof(OrganizationalUnitAddedToOrganizationalUnitEventTaskBuilder     ) },
            { typeof(OrganizationalUnitCreatedEvent                       ), typeof(OrganizationalUnitCreatedEventTaskBuilder                       ) },
            { typeof(OrganizationalUnitDeletedEvent                       ), typeof(OrganizationalUnitDeletedEventTaskBuilder                       ) },
            { typeof(OrganizationalUnitPropertyChangedEvent               ), typeof(OrganizationalUnitPropertyChangedEventTaskBuilder               ) },
            { typeof(OrganizationalUnitRemovedFromGroupEvent              ), typeof(OrganizationalUnitRemovedFromGroupEventTaskBuilder              ) },
            { typeof(OrganizationalUnitRemovedFromOrganizationalUnitEvent ), typeof(OrganizationalUnitRemovedFromOrganizationalUnitEventTaskBuilder ) },
            { typeof(RoleCreatedEvent                                     ), typeof(RoleCreatedEventTaskBuilder                                     ) },
            { typeof(RoleDeletedEvent                                     ), typeof(RoleDeletedEventTaskBuilder                                     ) },
            { typeof(RolePropertyChangedEvent                             ), typeof(RolePropertyChangedEventTaskBuilder                             ) },
            { typeof(UserAddedToGroupEvent                                ), typeof(UserAddedToGroupEventTaskBuilder                                ) },
            { typeof(UserAddedToOrganizationalUnitEvent                   ), typeof(UserAddedToOrganizationalUnitEventTaskBuilder                   ) },
            { typeof(UserAddedToOrganizationaRoleEvent                    ), typeof(UserAddedToOrganizationalRoleEventTaskBuilder                   ) },
            { typeof(UserCreatedEvent                                     ), typeof(UserCreatedEventTaskBuilder                                     ) },
            { typeof(UserDeletedEvent                                     ), typeof(UserDeletedEventTaskBuilder                                     ) },
            { typeof(UserDisabledEvent                                    ), typeof(UserDisabledEventTaskBuilder                                    ) },
            { typeof(UserEnabledEvent                                     ), typeof(UserEnabledEventTaskBuilder                                     ) },
            { typeof(UserPropertyChangedEvent                             ), typeof(UserPropertyChangedEventTaskBuilder                             ) },
            { typeof(UserRemovedFromGroupEvent                            ), typeof(UserRemovedFromGroupEventTaskBuilder                            ) },
            { typeof(UserRemovedFromOrganizationalRoleEvent               ), typeof(UserRemovedFromOrganizationalRoleEventTaskBuilder               ) },
            { typeof(UserRemovedFromOrganizationalUnitEvent               ), typeof(UserRemovedFromOrganizationalUnitEventTaskBuilder               ) }
        };

        public ISyncTaskBuilder GetTaskBuilder( IEvent evt )
        {
            Type eventType = evt.GetType();
            Type taskBuilderType = mapping[ eventType ];
            return Activator.CreateInstance( taskBuilderType ) as ISyncTaskBuilder;
        }
    }
}