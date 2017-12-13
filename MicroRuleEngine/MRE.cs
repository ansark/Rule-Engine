using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace MicroRuleEngine
{
    public class MRE
    {

        public List<string> messages = new List<string>();
        public bool AddMessage(string msg)
        {
            messages.Add(msg);
            return false;
        }
        private ExpressionType[] nestedOperators = new ExpressionType[] { ExpressionType.And, ExpressionType.AndAlso, ExpressionType.Or, ExpressionType.OrElse };




        public bool PassesRules<T>(IList<Rule> rules, T toInspect)
        {
            return this.CompileRules<T>(rules).Invoke(toInspect);
        }
        public static bool tester<T>(BinaryExpression CC)
        {
            return true;
        }
        public Func<T, bool> CompileRule<T>(Rule r)
        {
            var OrderType = Expression.Parameter(typeof(T));
            Expression expr = GetExpressionForRule<T>(r, OrderType);
            return Expression.Lambda<Func<T, bool>>(expr, OrderType).Compile();
        }

        Expression GetExpressionForRule<T>(Rule r, ParameterExpression OrderType)
        {
            ExpressionType nestedOperator;
            if (ExpressionType.TryParse(r.Operator, out nestedOperator) && nestedOperators.Contains(nestedOperator) && r.Rules != null && r.Rules.Any())
                return BuildNestedExpression<T>(r.Rules, OrderType, nestedOperator);
            else
                return BuildExpr<T>(r, OrderType);
        }

        public Func<T, bool> CompileRules<T>(IList<Rule> rules)
        {
            var paramUser = Expression.Parameter(typeof(T));
            var expr = BuildNestedExpression<T>(rules, paramUser, ExpressionType.And);
            return Expression.Lambda<Func<T, bool>>(expr, paramUser).Compile();
        }

        Expression BuildNestedExpression<T>(IList<Rule> rules, ParameterExpression param, ExpressionType operation)
        {
            List<Expression> expressions = new List<Expression>();
            foreach (var r in rules)
            {
                expressions.Add(GetExpressionForRule<T>(r, param));
            }

            Expression expr = BinaryExpression(expressions, operation);
            return expr;
        }
        //that
        Expression BinaryExpression(IList<Expression> expressions, ExpressionType operationType)
        {
            Func<Expression, Expression, Expression> methodExp = new Func<Expression, Expression, Expression>((x1, x2) => Expression.And(x1, x2));
            switch (operationType)
            {
                case ExpressionType.Or:
                    methodExp = new Func<Expression, Expression, Expression>((x1, x2) => Expression.Or(x1, x2));
                    break;
                case ExpressionType.OrElse:
                    methodExp = new Func<Expression, Expression, Expression>((x1, x2) => Expression.OrElse(x1, x2));
                    break;
                case ExpressionType.AndAlso:
                    methodExp = new Func<Expression, Expression, Expression>((x1, x2) => Expression.AndAlso(x1, x2));
                    break;
            }

            if (expressions.Count == 1)
                return expressions[0];
            Expression exp = methodExp(expressions[0], expressions[1]);
            for (int i = 2; expressions.Count > i; i++)
            {
                exp = methodExp(exp, expressions[i]);
            }
            return exp;
        }

        Expression AndExpressions(IList<Expression> expressions)
        {
            if (expressions.Count == 1)
                return expressions[0];
            Expression exp = Expression.And(expressions[0], expressions[1]);
            for (int i = 2; expressions.Count > i; i++)
            {
                exp = Expression.And(exp, expressions[i]);
            }
            return exp;
        }

        Expression OrExpressions(IList<Expression> expressions)
        {
            if (expressions.Count == 1)
                return expressions[0];
            Expression exp = Expression.Or(expressions[0], expressions[1]);
            for (int i = 2; expressions.Count > i; i++)
            {
                exp = Expression.Or(exp, expressions[i]);
            }
            return exp;
        }

        public Expression GetProperty<T>(ParameterExpression ordertype, string propname)
        {
            MemberExpression propertyOnOrder = null;
            String[] childProperties = propname.Split('.');
            var property = typeof(T).GetProperty(childProperties[0]);
            var paramExp = Expression.Parameter(typeof(T), "SomeObject");

            propertyOnOrder = Expression.PropertyOrField(ordertype, childProperties[0]);
            for (int i = 1; i < childProperties.Length; i++)
            {
                var orig = property;
                property = property.PropertyType.GetProperty(childProperties[i]);
                if (property != null)
                    propertyOnOrder = Expression.PropertyOrField(propertyOnOrder, childProperties[i]);
            }
            //propType = propertyOnOrder.Type;
            return propertyOnOrder;
        }
        Expression BuildExpr<T>(Rule r, ParameterExpression OrderType)
        {
            Expression propertyOnOrder = null;
            Type propType = null;

            ExpressionType tBinary;
            if (string.IsNullOrEmpty(r.MemberName))//check is against the object itself
            {
                propertyOnOrder = OrderType;
                propType = propertyOnOrder.Type;
            }
            else if (r.MemberName.Contains('.'))//Child property
            {
                String[] childProperties = r.MemberName.Split('.');
                var property = typeof(T).GetProperty(childProperties[0]);
                var paramExp = Expression.Parameter(typeof(T), "SomeObject");

                propertyOnOrder = Expression.PropertyOrField(OrderType, childProperties[0]);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    var orig = property;
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    if (property != null)
                        propertyOnOrder = Expression.PropertyOrField(propertyOnOrder, childProperties[i]);
                }
                propType = propertyOnOrder.Type;
            }
            else//Property
            {
                propertyOnOrder = Expression.PropertyOrField(OrderType, r.MemberName);
                propType = propertyOnOrder.Type;
            }

            // is the operator a known .NET operator?
            if (ExpressionType.TryParse(r.Operator, out tBinary) || r.Operator == "Function")
            {
                //no repetition of error message,action or setter found
                Expression right = null;
                BinaryExpression binary = null;
                MethodCallExpression ErrorMessage = Expression.Call(OrderType, "AddMessage", null, new Expression[] { Expression.Constant(r.ErrorMessage) });
                if (r.Operator != "Function")
                {
                    right = this.StringToExpression(r.TargetValue, propType);
                    binary = Expression.MakeBinary(tBinary, propertyOnOrder, right);
                   
                }
                //function
                if (!string.IsNullOrEmpty(r.Function))
                {
                    // var firstexp = Expression.MakeBinary(tBinary, propertyOnOrder, right);
                    Expression[] functionParams = new Expression[] { Expression.Constant(r.ErrorMessage) };

                    Expression FunctionCall = Expression.Call(OrderType, r.Function, null, r.Inputs.Select(x => Expression.Constant(x)).ToArray());

                    if (r.Operator == "Function")
                    {
                        //call functon and if the function returns false, set error message

                        //Expression.IfThenElse(FunctionCall, Expression.Constant(true)


                      return  Expression.Condition(FunctionCall, Expression.Constant(true),
                            Expression.Block(ErrorMessage, Expression.Condition(Expression.Constant(r.ReturnTue),
                            Expression.Constant(true), Expression.Constant(true))));
                    }
                    return Expression.Block(Expression.IfThenElse(binary, FunctionCall, ErrorMessage),
                         Expression.Condition(Expression.Constant(r.ReturnTue), Expression.Condition(binary, binary, Expression.Not(binary)), binary)
                        );//need fix-u dont 
                    //
                }
                //setter
                else if (!string.IsNullOrEmpty(r.Setter))
                {
                    //no repetition of error message,action or setter found
                    var prop1 = Expression.PropertyOrField(OrderType, r.Setter.Split('=')[0]);
                    var prop2 = Expression.Constant(r.Setter.Split('=')[1]);
                    return Expression.Block(Expression.IfThenElse(binary, Expression.Assign(prop1, prop2), ErrorMessage),
                        Expression.Condition(Expression.Constant(r.ReturnTue), Expression.Condition(binary, binary, Expression.Not(binary)), binary)
                        );
                    //
                }
                //
                else
                {
                    return Expression.Block(Expression.IfThen(Expression.Not(binary), ErrorMessage),
                      Expression.Condition(Expression.Constant(r.ReturnTue), Expression.Condition(binary, binary, Expression.Not(binary)), binary)
                        );
                    //var ifthen = Expression.IfThen(Expression.Not(binary), ErrorMessage);
                    //return Expression.Block(ifthen, binary);
                }
            }
            else if (r.Operator == "IsMatch")
            {
                var tester = Expression.Call(
                    typeof(Regex).GetMethod("IsMatch",
                        new[] { typeof(string), typeof(string), typeof(RegexOptions) }),
                    propertyOnOrder,
                    Expression.Constant(r.TargetValue, typeof(string)),
                    Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions))
                );

                return Expression.Block(Expression.IfThen(tester,
                  Expression.Call(OrderType, "AddMessage", null, new Expression[] { Expression.Constant(r.MemberName) })), tester);
            }
            else //Invoke a method on the Property
            {
                Type[] parameterTypes = r.Inputs.Select(x => x.GetType()).ToArray();
                var methodInfo = propType.GetMethod(r.Operator, parameterTypes);
                if (!methodInfo.IsGenericMethod)
                    parameterTypes = null;//Only pass in type information to a Generic Method
                Expression[] prametersAsArray = r.Inputs.Select(x => Expression.Constant(x)).ToArray();
                var firstcall = Expression.Call(propertyOnOrder, r.Operator, parameterTypes, prametersAsArray);

                var message = Expression.Call(OrderType, "AddMessage", null, new Expression[] { Expression.Constant(r.MemberName) });
                var ifthen = Expression.IfThen(Expression.Not(firstcall), message);
                return Expression.Block(ifthen, firstcall);
            }
        }

        private Expression StringToExpression(string value, Type propType)
        {
            ConstantExpression right = null;

            if (value == null || value.ToLower() == "null")
            {
               
                right = Expression.Constant(null);
            }
            else
            {
                
                right = Expression.Constant(Convert.ChangeType(value, propType));
            }
            return right;
        }
    }

    public class Rule
    {
        public Rule()
        {
            Inputs = new List<object>();
        }
        public string MemberName { get; set; }
        public string Operator { get; set; }
        public string TargetValue { get; set; }
        public List<Rule> Rules { get; set; }
        public List<object> Inputs { get; set; }
        public string Function { get; set; }
        private string _ErrorMessage;
        public string ErrorMessage
        {

            get
            {
                if (_ErrorMessage == null)
                {
                    _ErrorMessage = string.Empty;
                }
                return _ErrorMessage;
            }

            set
            {
                _ErrorMessage = value;
            }
        }
        public bool ReturnTue { get; set; }
        public string Setter { get; set; }//BE=34
        //Rule=Operator=function,function="functionname",parms=object array
    }

    public class RuleValue<T>
    {
        public T Value { get; set; }
        public List<Rule> Rules { get; set; }
    }

    public class RuleValueString : RuleValue<string> { }
}
