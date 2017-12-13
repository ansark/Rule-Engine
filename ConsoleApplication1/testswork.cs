
//            return;

//            ParameterExpression OrdertypeAsExpression = Expression.Parameter(typeof(Appointment));
//Expression Indentifier = Expression.PropertyOrField(OrdertypeAsExpression, "Flow");
//Expression Right = Expression.PropertyOrField(OrdertypeAsExpression, "BE");
//Expression beAndFlow = Expression.MakeBinary(ExpressionType.Equal, Indentifier, Right);

//var assigment = Expression.Assign(Right, Expression.Constant("BECHANGED"));

//var newlast = Expression.Block(beAndFlow, Expression.IfThen(beAndFlow, assigment), beAndFlow);
////Expression.Block(expression)



//Expression cccc = Expression.Parameter(typeof(Appointment));
//Type[] paramtypes = new Type[] { typeof(int) };
//paramtypes = null;
//            Expression[] functionParams = new Expression[] { Expression.Constant("some message") };
//var CallerEXp = Expression.Call(OrdertypeAsExpression, "AddMessage", paramtypes, functionParams);


//var lastExpression = Expression.Block(CallerEXp, newlast);


////if ContainerNumber=1, set booking number=BookingNumber=3

////Expression.Block(expres)



//Func<Appointment, bool> abcd = Expression.Lambda<Func<Appointment, bool>>(lastExpression, OrdertypeAsExpression).Compile();

//MRE d = new MRE();
//bool done = abcd(app1);
//List<string> str = d.messages;




//Rule ruleCreateRequest = new Rule()
//{
//    Operator = "AndAlso",
//    Rules = new List<Rule>()
//                {
//                    new Rule() { Operator="CheckLength",Inputs=new List<object> {7 } },
//                    new Rule { Operator="AppointMentMaxDays",Inputs=new List<object> {6 }},
//                    new Rule { },
//                    new Rule
//                    {
//                        Operator = "OrElse",
//                          Rules = new List<Rule>()
//                          {
//                                new Rule(){
//                                    Operator="AndAlso",
//                                    Rules=new List<Rule>
//                                    {
//                                        new Rule { MemberName="Flow",TargetValue="Import",Operator="Equal"},
//                                        new Rule
//                                        {
//                                            Operator="AndAlso",
//                                            Rules=new List<Rule>
//                                            {
//                                                new Rule
//                                                {
//                                                    Operator="OrElse",
//                                                    Rules=new List<Rule>()
//                                                    {
//                                                             new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual"},
//                                                             new Rule { MemberName="BE",TargetValue=null,Operator="NotEqual"}
//                                                    }
//                                                },
//                                                new Rule
//                                                {
//                                                    Operator="OrElse",
//                                                    Rules =new List<Rule>()
//                                                    {
//                                                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual"},
//                                                        new Rule { MemberName="BE",TargetValue=string.Empty,Operator="NotEqual"}
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                },
//                                new Rule(){
//                                    Operator="AndAlso",
//                                    Rules=new List<Rule>
//                                    {
//                                        new Rule { MemberName="Flow",TargetValue="Export",Operator="Equal"},
//                                        new Rule
//                                        {
//                                            Operator="AndAlso",
//                                            Rules=new List<Rule>
//                                            {
//                                                new Rule
//                                                {
//                                                    Operator="OrElse",
//                                                    Rules=new List<Rule>()
//                                                    {
//                                                             new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual"},
//                                                             new Rule { MemberName="BookingNumber",TargetValue=null,Operator="NotEqual"}
//                                                    }
//                                                },
//                                                new Rule
//                                                {
//                                                    Operator="OrElse",
//                                                    Rules =new List<Rule>()
//                                                    {
//                                                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual"},
//                                                        new Rule { MemberName="BookingNumber",TargetValue=string.Empty,Operator="NotEqual"}
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                },
//                                new Rule(){
//                                    Operator="AndAlso",
//                                    Rules=new List<Rule>
//                                    {
//                                        new Rule { MemberName="Flow",TargetValue="Special",Operator="Equal"},
//                                        new Rule
//                                        {
//                                            Operator="AndAlso",
//                                            Rules=new List<Rule>
//                                            {
//                                                new Rule
//                                                {
//                                                    Operator="OrElse",
//                                                    Rules=new List<Rule>()
//                                                    {
//                                                             new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual"},
//                                                             new Rule { MemberName="BookingNumber",TargetValue=null,Operator="NotEqual"}
//                                                    }
//                                                },
//                                                new Rule
//                                                {
//                                                    Operator="OrElse",
//                                                    Rules =new List<Rule>()
//                                                    {
//                                                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual"},
//                                                        new Rule { MemberName="BookingNumber",TargetValue=string.Empty,Operator="NotEqual"}
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                         }
//                    }
//                }
//};

//XmlSerializer serializer1 = new XmlSerializer(typeof(Rule));
//            using (TextWriter writer = new StreamWriter(@"C:\dd\CreateRequest.xml"))
//            {
//                serializer1.Serialize(writer, ruleCreateRequest);
//            }


//            Rule ruleSendAppointment = new Rule()
//            {
//                Operator = "AndAlso",
//                Rules = new List<Rule>()
//                {
//                    new Rule() {MemberName="Date",TargetValue=null,Operator="NotEqual"},
//                    new Rule() {MemberName="TimeSlotFrom",TargetValue=null,Operator ="NotEqual"},
//                    new Rule() {MemberName="TimeSlotTo",TargetValue=null,Operator ="NotEqual" },
//                    new Rule() {MemberName="AlreadyBooked",TargetValue="true",Operator ="NotEqual" },
//                    new Rule()
//                    {
//                       Operator="OrElse",
//                       Rules=new List<Rule>()
//                       {
//                           new Rule
//                           {
//                                Operator="AndAlso",
//                                Rules=new List<Rule>()
//                                {
//                                    new Rule {MemberName="TransactionType",TargetValue="DI",Operator="Equal" },
//                                    new Rule {MemberName="Flow",TargetValue="Import",Operator="Equal" },
//                                    new Rule {MemberName="IDO",TargetValue=null,Operator="NotEqual" },
//                                    new Rule {MemberName="IDO",TargetValue=string.Empty,Operator="NotEqual" }
//                                }
//                           },
//                           new Rule
//                           {
//                               Operator="OrElse",
//                               Rules=new List<Rule>()
//                               {
//                                   new Rule {MemberName="TransactionType",TargetValue="DI",Operator="NotEqual" },
//                                    new Rule {MemberName="Flow",TargetValue="Import",Operator="NotEqual" }
//                               }
//                           }
//                       }

//                    }
//                }
//            };

//XmlSerializer serializer2 = new XmlSerializer(typeof(Rule));
//            using (TextWriter writer = new StreamWriter(@"C:\dd\sendAppintment.xml"))
//            {
//                serializer2.Serialize(writer, ruleSendAppointment);
//            }

//            Rule ruleCharges = new Rule
//            {
//                Operator = "AndAlso",
//                Rules = new List<Rule>()
//               {
//                    new Rule() {MemberName="Date",TargetValue=null,Operator="NotEqual"},
//                    new Rule() {MemberName="TimeSlotFrom",TargetValue=null,Operator ="NotEqual"},
//                    new Rule() {MemberName="TimeSlotTo",TargetValue=null,Operator ="NotEqual" },
//                    new Rule() { MemberName="TruckType",TargetValue=null,Operator ="NotEqual"},
//                    new Rule() { MemberName="TruckType",TargetValue=string.Empty,Operator ="NotEqual" },
//                    new Rule() { MemberName="DriverContact",TargetValue=null,Operator ="NotEqual" },
//                    new Rule() { MemberName="DriverContact",TargetValue=string.Empty,Operator ="NotEqual" },
//                    new Rule() { MemberName="DriverName",TargetValue=string.Empty,Operator ="NotEqual" },
//                    new Rule() { MemberName="DriverName",TargetValue=null,Operator ="NotEqual" },
//                    new Rule() { MemberName="NotificationType",TargetValue=string.Empty,Operator ="NotEqual" },
//                    new Rule() { MemberName="NotificationType",TargetValue=null,Operator ="NotEqual" },
//                    new Rule() { MemberName="AlreadyBooked",TargetValue="No",Operator ="Equal" }

//               }
//            };
//XmlSerializer serializer3 = new XmlSerializer(typeof(Rule));
//            using (TextWriter writer = new StreamWriter(@"C:\dd\Charges.xml"))
//            {
//                serializer3.Serialize(writer, ruleCharges);
//            }

//            Appointment ap = new Appointment();
//ap.Flow = "Import";
//            ap.BE = "3";

//            MRE engine = new MRE();
//var boolMethod = engine.CompileRule<Appointment>(ruleCreateRequest);
//bool passes = boolMethod(ap);
//Expression ceee = Expression.Constant(9);





////new Rule()
////{
////    Operator = "AndAlso",
////    Rules = new List<Rule>
////            {
////                new Rule { MemberName="Flow",TargetValue="Export",Operator="Equal"},
////                new Rule
////                {
////                    Operator="OrElse",
////                    Rules=new List<Rule>
////                    {
////                        new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual"},
////                        new Rule { MemberName="BookingNumber",TargetValue=null,Operator="NotEqual"},
////                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual"},
////                        new Rule { MemberName="BookingNumber",TargetValue=string.Empty,Operator="NotEqual"}
////                    }
////                }

////            }
////},
////         new Rule()
////         {
////             Operator = "AndAlso",
////             Rules = new List<Rule>
////            {
////                new Rule { MemberName="Flow",TargetValue="Special",Operator="Equal"},
////                new Rule
////                {
////                    Operator="OrElse",
////                    Rules=new List<Rule>
////                    {
////                        new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual"},
////                        new Rule { MemberName="BookingNumber",TargetValue=null,Operator="NotEqual"},
////                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual"},
////                        new Rule { MemberName="BookingNumber",TargetValue=string.Empty,Operator="NotEqual"}
////                    }
////                }

////            }
////         },



//Rule rule = new Rule()
//{
//    Operator = "Equal",
//    MemberName = "FirstName",
//    TargetValue = "John",

//};
//Expression.Constant(rule.TargetValue);
//            //Expression.Constant(s                                          

//            //MRE engine = new MRE();
//            //var boolMethod = engine.CompileRule<Order>(rule);
//            //bool passes = boolMethod(order);







//            BooleanMethods();
//            //int num = 100;
//            //Expression conditionExpr = Expression.Condition(
//            //               Expression.Constant(num > 10),
//            //               Expression.Constant(true),
//            //               Expression.Constant(false)
//            //             );

//            //Console.WriteLine(conditionExpr.ToString());

//            //// The following statement first creates an expression tree,
//            //// then compiles it, and then executes it.       
//            //Console.WriteLine(
//            //    Expression.Lambda<Func<bool>>(conditionExpr).Compile()());

//            //Console.Read();
//            ConditionalLogic();
////ConditionalLogic();