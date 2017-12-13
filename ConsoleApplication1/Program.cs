using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MicroRuleEngine
{

    public interface ABC
    {
        string getMessage();
        string AddMessage(string str);


    }
    class Program
    {

  
        static  string compileRule<T>(Rule r,T app)
        {
            bool status = false;
            if (r.Rules ==null)
            {
                MRE m = new MRE();
                var newfunction = m.CompileRule<T>(r);
              status   = newfunction(app);
            }
            else
            {
                foreach (Rule item in r.Rules)
                {
                    compileRule<T>(item, app);
                }  
            }
            if (!status)
            {
                return ((ABC)app).getMessage();
            }
            else
            {
                return string.Empty;
            }
        }

        static void Main(string[] args)
        {

            //only first message will be displayed.
            //sample code to display all messages added
            //setter will set a value to a property if the specified condition is satisfied, In setter,return true can be used if u want it to return true even if its false
            //
            //objectives
            //1.All setter and actions should be executed exactly once. "False event" should be raised once and once only-seems correct now
            //2. add "return true" functionality without causing the expression to be executed twice-done
            //3.Provide UI to serialize a class and test it. 
            //4.Provide XML & Json serilization option
            //5. add more functionality

            Appointment app1 = new Appointment();
            app1.Flow = "Import";
            app1.BE = "asd";
            app1.ContainerNumber = string.Empty;
            Rule ruleCreateRequest = new Rule()
            {
                Operator = "AndAlso",
                Rules = new List<Rule>()
                {
                    new Rule() {  Operator="Function"  ,Function="CheckLength",ErrorMessage="Incorrect length" },
                    new Rule {  Operator="Function", Function="AppointMentMaxDays", ErrorMessage="Appointment is not in the allowed window of date"},
                    new Rule
                    {
                        Operator = "OrElse",
                          Rules = new List<Rule>()
                          {
                                new Rule(){
                                    Operator="AndAlso",
                                    Rules=new List<Rule>
                                    {
                                        new Rule { MemberName="Flow",TargetValue="Import",Operator="Equal"},
                                        new Rule
                                        {
                                            Operator="AndAlso",
                                            Rules=new List<Rule>
                                            {
                                                new Rule
                                                {
                                                    Operator="OrElse",
                                                    Rules=new List<Rule>()
                                                    {
                                                             new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual", ErrorMessage= "Container number is null"},
                                                             new Rule { MemberName="BE",TargetValue=null,Operator="NotEqual",ErrorMessage="BE number is null"}
                                                    }
                                                },
                                                new Rule
                                                {
                                                    Operator="OrElse",
                                                    Rules =new List<Rule>()
                                                    {
                                                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual",ErrorMessage="Container number is empty"},
                                                        new Rule { MemberName="BE",TargetValue=string.Empty,Operator="NotEqual", ErrorMessage="BE number is empty"}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new Rule(){
                                    Operator="AndAlso",
                                    Rules=new List<Rule>
                                    {
                                        new Rule { MemberName="Flow",TargetValue="Export",Operator="Equal"},
                                        new Rule
                                        {
                                            Operator="AndAlso",
                                            Rules=new List<Rule>
                                            {
                                                new Rule
                                                {
                                                    Operator="OrElse",
                                                    Rules=new List<Rule>()
                                                    {
                                                             new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual",ErrorMessage="Container number is null"},
                                                             new Rule { MemberName="BookingNumber",TargetValue=null,Operator="NotEqual",ErrorMessage="Booking number is null"}
                                                    }
                                                },
                                                new Rule
                                                {
                                                    Operator="OrElse",
                                                    Rules =new List<Rule>()
                                                    {
                                                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual",ErrorMessage="Container number is empty"},
                                                        new Rule { MemberName="BookingNumber",TargetValue=string.Empty,Operator="NotEqual",ErrorMessage="Booking number is empty"}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new Rule(){
                                    Operator="AndAlso",
                                    Rules=new List<Rule>
                                    {
                                        new Rule { MemberName="Flow",TargetValue="Special",Operator="Equal"},
                                        new Rule
                                        {
                                            Operator="AndAlso",
                                            Rules=new List<Rule>
                                            {
                                                new Rule
                                                {
                                                    Operator="OrElse",
                                                    Rules=new List<Rule>()
                                                    {
                                                             new Rule { MemberName="ContainerNumber",TargetValue=null,Operator="NotEqual",ErrorMessage="3. Container number is null"},
                                                             new Rule { MemberName="BookingNumber",TargetValue=null,Operator="NotEqual",ErrorMessage="3.Booking number is null"}
                                                    }
                                                },
                                                new Rule
                                                {
                                                    Operator="OrElse",
                                                    Rules =new List<Rule>()
                                                    {
                                                        new Rule { MemberName="ContainerNumber",TargetValue=string.Empty,Operator="NotEqual",ErrorMessage="3.Container number is empty"},
                                                        new Rule { MemberName="BookingNumber",TargetValue=string.Empty,Operator="NotEqual",ErrorMessage="3.Booking number is empty"}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                
                         }
                    }
                }
            };
            Rule newRule = new Rule()
            {
                Operator = "AndAlso",
                Rules = new List<Rule>()
                {
                     new Rule(){ MemberName="Flow",Operator="Equal",TargetValue="Import",Function="SetCharges",Inputs=new List<object> {"34.5"},ErrorMessage="Incorrect flow",ReturnTue=true },
                     new Rule { MemberName="ContainerNumber",TargetValue="",Operator="NotEqual",Setter="ContainerNumber=CONT123",ErrorMessage="Container number not provided", ReturnTue=true},
                     new Rule { MemberName="BE",TargetValue="",Operator="NotEqual",ErrorMessage="BE number is not provided",ReturnTue =true}
                }
            };

            MRE m = new MRE();
            var newfunction= m.CompileRule<Appointment>(ruleCreateRequest);
            var status=  newfunction(app1);



            Rule ruleSendAppointment = new Rule()
            {
                Operator = "AndAlso",
                Rules = new List<Rule>()
                {
                    new Rule() {MemberName="Date",TargetValue="12-jan-2019",Operator="NotEqual"},
                    new Rule() {MemberName="TimeSlotFrom",TargetValue=null,Operator ="NotEqual"},
                    new Rule() {MemberName="TimeSlotTo",TargetValue=null,Operator ="NotEqual" },
                    new Rule() {MemberName="AlreadyBooked",TargetValue="true",Operator ="NotEqual" },
                    new Rule()
                    {
                       Operator="OrElse",
                       Rules=new List<Rule>()
                       {
                           new Rule
                           {
                                Operator="AndAlso",
                                Rules=new List<Rule>()
                                {
                                    new Rule {MemberName="TransactionType",TargetValue="DI",Operator="Equal" },
                                    new Rule {MemberName="Flow",TargetValue="Import",Operator="Equal" },
                                    new Rule {MemberName="IDO",TargetValue=null,Operator="NotEqual" },
                                    new Rule {MemberName="IDO",TargetValue=string.Empty,Operator="NotEqual" }
                                }
                           },
                           new Rule
                           {
                               Operator="OrElse",
                               Rules=new List<Rule>()
                               {
                                   new Rule {MemberName="TransactionType",TargetValue="DI",Operator="NotEqual" },
                                    new Rule {MemberName="Flow",TargetValue="Import",Operator="NotEqual" }
                               }
                           }
                       }

                    }
                }
            };
            Rule ruleCharges = new Rule
            {
                Operator = "AndAlso",
                Rules = new List<Rule>()
               {
                    new Rule() {MemberName="Date",TargetValue=null,Operator="NotEqual"},
                    new Rule() {MemberName="TimeSlotFrom",TargetValue=null,Operator ="NotEqual"},
                    new Rule() {MemberName="TimeSlotTo",TargetValue=null,Operator ="NotEqual" },
                    new Rule() { MemberName="TruckType",TargetValue=null,Operator ="NotEqual"},
                    new Rule() { MemberName="TruckType",TargetValue=string.Empty,Operator ="NotEqual" },
                    new Rule() { MemberName="DriverContact",TargetValue=null,Operator ="NotEqual" },
                    new Rule() { MemberName="DriverContact",TargetValue=string.Empty,Operator ="NotEqual" },
                    new Rule() { MemberName="DriverName",TargetValue=string.Empty,Operator ="NotEqual" },
                    new Rule() { MemberName="DriverName",TargetValue=null,Operator ="NotEqual" },
                    new Rule() { MemberName="NotificationType",TargetValue=string.Empty,Operator ="NotEqual" },
                    new Rule() { MemberName="NotificationType",TargetValue=null,Operator ="NotEqual" },
                    new Rule() { MemberName="AlreadyBooked",TargetValue="No",Operator ="Equal" }

               }
            };

            app1 = new Appointment();

            app1.Flow = "Import";
            app1.BE = string.Empty;
            app1.ContainerNumber = string.Empty;

            m = new MRE();
             newfunction = m.CompileRule<Appointment>(ruleSendAppointment);
             status = newfunction(app1);
            //compileRule<Appointment>(ruleCreateRequest, app1);


            //Console.WriteLine(status.ToString().ToUpper());
            //Console.WriteLine("---------------------------");

            foreach (var item in app1.messages)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine("---------------------------");
            //foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(app1))
            //{
            //    string name = descriptor.Name;
            //    object value = descriptor.GetValue(app1);
            //    Console.WriteLine("{0}={1}", name, value);
            //}

            Console.Read();
        }
    }
}
