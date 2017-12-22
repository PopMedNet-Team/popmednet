using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;
using System.Linq.Expressions;
//using Lpp.Data.Composition;
using Lpp.Audit.Data;

namespace Lpp.Audit
{
    public static class Aud
    {
        public static IModule Module<TDomain>()
        {
            return new ModuleBuilder()
                .Export<IAuditService<TDomain>, AuditService<TDomain>>()
                .Export<INotificationService<TDomain>, NotificationService<TDomain>>()
                //.Export<IPersistenceDefinition<TDomain>, PersistenceDefinition<TDomain>>()
                .CreateModule();
        }

        public static AuditEventKind Event(Guid id, string name, Security.SecurityTargetKind appliesTo, IEnumerable<IAuditProperty> props)
        {
            return new AuditEventKind(id, name, appliesTo, props);
        }

        public static AuditEventKind Event(string id, string name, Security.SecurityTargetKind appliesTo, params IAuditProperty[] props)
        {
            return new AuditEventKind(new Guid(id), name, appliesTo, props);
        }

        public static IAuditProperty<T> Property<T>(Guid id, string name)
        {
            return new AuditProperty<T>(id, name);
        }

        public static IAuditProperty<T> Property<T>(string id, string name)
        {
            return new AuditProperty<T>(new Guid(id), name);
        }

        public static AuditEventKind Event<T>() { return CodeFirst.GetKind<T>(); }
        public static AuditEventKind DeclareEvent<T>(Security.SecurityTargetKind appliesTo) { return CodeFirst.CreateKind<T>(appliesTo); }
        public static IAuditProperty<T> Property<THost, T>(Expression<Func<THost, T>> accessor) { return CodeFirst.Property(accessor); }
        public static IAuditProperty UntypedProperty<THost, T>(Expression<Func<THost, T>> accessor) { return CodeFirst.UntypedProperty(accessor); }

        public static TEvent As<TEvent>(this Data.AuditEvent ev) where TEvent : class, new() { return CodeFirst.As<TEvent>(ev); }
        public static void Assign<TEvent>(this Data.AuditEvent to, TEvent from) where TEvent : class { to.PropertyValues = AsPropertyValues(from).ToList(); }
        public static IEnumerable<Data.AuditPropertyValue> AsPropertyValues<TEvent>(TEvent from) where TEvent : class { return CodeFirst.AsPropertyValues(from); }
    }
}