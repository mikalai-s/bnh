//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Web;
//using MongoDB.Bson.Serialization;

//namespace Ms.Cms
//{
//    internal class DynamicBsonClassMap : BsonClassMap
//    {
//            // Methods
//        public DynamicBsonClassMap(Type type) : base(type)
//        {
//        }

//        private static MemberInfo GetMemberInfoFromLambda<TMember>(Expression<Func<TClass, TMember>> memberLambda)
//        {
//            MemberExpression operand;
//            Expression body = memberLambda.Body;
//            ExpressionType nodeType = body.NodeType;
//            if (nodeType != ExpressionType.Convert)
//            {
//                if (nodeType != ExpressionType.MemberAccess)
//                {
//                    throw new BsonSerializationException("Invalid lambda expression");
//                }
//                operand = (MemberExpression) body;
//            }
//            else
//            {
//                UnaryExpression expression3 = (UnaryExpression) body;
//                operand = (MemberExpression) expression3.Operand;
//            }
//            MemberInfo member = operand.Member;
//            MemberTypes memberType = member.MemberType;
//            if ((memberType != MemberTypes.Field) && (memberType != MemberTypes.Property))
//            {
//                throw new BsonSerializationException("Invalid lambda expression");
//            }
//            return member;
//        }

//        public BsonMemberMap GetMemberMap<TMember>(Expression<Func<TClass, TMember>> memberLambda)
//        {
//            string memberNameFromLambda = BsonClassMap<TClass>.GetMemberNameFromLambda<TMember>(memberLambda);
//            return base.GetMemberMap(memberNameFromLambda);
//        }

//        private static string GetMemberNameFromLambda<TMember>(Expression<Func<TClass, TMember>> memberLambda)
//        {
//            return BsonClassMap<TClass>.GetMemberInfoFromLambda<TMember>(memberLambda).Name;
//        }

//        public BsonMemberMap MapExtraElementsField<TMember>(Expression<Func<TClass, TMember>> fieldLambda)
//        {
//            BsonMemberMap memberMap = this.MapField<TMember>(fieldLambda);
//            base.SetExtraElementsMember(memberMap);
//            return memberMap;
//        }

//        public BsonMemberMap MapExtraElementsMember<TMember>(Expression<Func<TClass, TMember>> memberLambda)
//        {
//            BsonMemberMap memberMap = this.MapMember<TMember>(memberLambda);
//            base.SetExtraElementsMember(memberMap);
//            return memberMap;
//        }

//        public BsonMemberMap MapExtraElementsProperty<TMember>(Expression<Func<TClass, TMember>> propertyLambda)
//        {
//            BsonMemberMap memberMap = this.MapProperty<TMember>(propertyLambda);
//            base.SetExtraElementsMember(memberMap);
//            return memberMap;
//        }

//        public BsonMemberMap MapField<TMember>(Expression<Func<TClass, TMember>> fieldLambda)
//        {
//            return this.MapMember<TMember>(fieldLambda);
//        }

//        public BsonMemberMap MapIdField<TMember>(Expression<Func<TClass, TMember>> fieldLambda)
//        {
//            BsonMemberMap memberMap = this.MapField<TMember>(fieldLambda);
//            base.SetIdMember(memberMap);
//            return memberMap;
//        }

//        public BsonMemberMap MapIdMember<TMember>(Expression<Func<TClass, TMember>> memberLambda)
//        {
//            BsonMemberMap memberMap = this.MapMember<TMember>(memberLambda);
//            base.SetIdMember(memberMap);
//            return memberMap;
//        }

//        public BsonMemberMap MapIdProperty<TMember>(Expression<Func<TClass, TMember>> propertyLambda)
//        {
//            BsonMemberMap memberMap = this.MapProperty<TMember>(propertyLambda);
//            base.SetIdMember(memberMap);
//            return memberMap;
//        }

//        public BsonMemberMap MapMember<TMember>(Expression<Func<TClass, TMember>> memberLambda)
//        {
//            MemberInfo memberInfoFromLambda = BsonClassMap<TClass>.GetMemberInfoFromLambda<TMember>(memberLambda);
//            return base.MapMember(memberInfoFromLambda);
//        }

//        public BsonMemberMap MapProperty<TMember>(Expression<Func<TClass, TMember>> propertyLambda)
//        {
//            return this.MapMember<TMember>(propertyLambda);
//        }

//        public void UnmapField<TMember>(Expression<Func<TClass, TMember>> fieldLambda)
//        {
//            this.UnmapMember<TMember>(fieldLambda);
//        }

//        public void UnmapMember<TMember>(Expression<Func<TClass, TMember>> memberLambda)
//        {
//            MemberInfo memberInfoFromLambda = BsonClassMap<TClass>.GetMemberInfoFromLambda<TMember>(memberLambda);
//            base.UnmapMember(memberInfoFromLambda);
//        }

//        public void UnmapProperty<TMember>(Expression<Func<TClass, TMember>> propertyLambda)
//        {
//            this.UnmapMember<TMember>(propertyLambda);
//        }
//    }
//}