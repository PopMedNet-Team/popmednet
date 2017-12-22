using Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model;
using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class ExpressionTreeTests
    {
        readonly int[] BaseIntArray = new[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 1, 2, 3, 4, 1, 2, 3 };
        static readonly string ConnectionString;
        static readonly string OracleConnectionString;

        static readonly log4net.ILog Logger;

        static ExpressionTreeTests()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;
            OracleConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_ORACLE"].ConnectionString;

            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(ExpressionTreeTests));
        }


        [TestMethod]
        public void ObservationPeriodExistsExample2()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                var dtStartRange = new DateTime(2000, 1, 1);
                var dtEndRange = new DateTime(2010, 1, 1);

                var Vitals = from v in db.Vitals where (v.Encounter.AdmittedOn >= dtStartRange && v.Encounter.AdmittedOn <= dtEndRange) select v; //This is done this way so that you could do an IF that returns either vitals straight up or Vitals with encounter factored in. You could do this for all other cases where Encounter is on the table you're looking at.

                var patients = from p in db.Patients
                               let vitals = (from v in Vitals where v.PatientID == p.ID && v.Height >= 160 && v.Height <= 200 select v)
                               let vital = vitals.OrderByDescending(v => v.MeasuredOn).FirstOrDefault()
                               where vitals.Any()
                               select new { p.ID, p.Race, p.Sex, vital.Height, vital.Weight };

                Console.WriteLine(patients.Expression.ToString());
                             

            }
        }

        [TestMethod]
        public void ObservationPeriodExistsExample()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                var dtStartRange = new DateTime(2000, 1, 1);
                var dtEndRange = new DateTime(2010, 1, 1);
                var patients = from p in db.Patients
                               join e in db.Encounters on p.ID equals e.PatientID
                               where e.AdmittedOn >= dtStartRange && e.AdmittedOn <= dtEndRange
                               select new PatientBase { Patient = p, EncounterID = e.ID };

                var rawVitals = (from p in patients
                                 let v = p.Patient.Vitals.Where(v => v.Height > 160 && v.Height < 190 && (p.EncounterID == null || p.EncounterID == v.EncounterID))
                                 let vital = v.OrderByDescending(vit => vit.MeasuredOn).FirstOrDefault()
                                 where v.Any()
                                 select new { Patient = p.Patient, Vital = vital }).Distinct();
                var vitals = rawVitals.Select(p => new { p.Patient.ID, p.Patient.Race, p.Patient.Sex, Height = p.Vital.Height, p.Vital.Weight, p.Vital.BMI });


                /* Comments
                 * Because it's using PatientBase (see below) everything else can use it blindly even if you doing join on the encounter table in the case where there is no Observation Period.
                 * If EncounterID == null then ignore, else join on other table
                 * 
                 * Obviously I just did a patient list with their Height, weight etc. as of the last vitals that was taken. You'll of course put in groups etc. but note the where which works off of the let so that it's always using the most recent vitals data and it's the same one in the where as the select instead of using an any.
                 */
            }
        }


        public class PatientBase
        {
            public Patient Patient { get; set; }
            public string EncounterID { get; set; }
        }

        [TestMethod]
        public void CountIQueryable()
        {
            var query = BaseIntArray.AsQueryable();

            //Expression call = Expression.Call(Expression.Constant(BaseIntArray), typeof(Queryable).GetMethod("Count", new []{typeof(int)}));
            //Console.WriteLine(call.ToString());

            //var methodInfos = typeof(Queryable).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Where(m => m.Name.Equals("Count") && m.GetParameters().Count() == 1).First();
            //var callMI = methodInfos.MakeGenericMethod(new[] { typeof(int) });
            //Console.WriteLine(callMI.ToString());

            //Expression call = Expression.Call(callMI, Expression.Constant(query));
            //Console.WriteLine(call.ToString());

            //var obj = query.Provider.Execute(call);
            //Console.WriteLine(obj);

            //Assert.AreEqual(query.Count(), obj);

            Expression callExpress = Expression.Call(typeof(Queryable), "Count", new Type[] { query.ElementType }, query.Expression);
            Console.WriteLine(callExpress);
            var cc = query.Provider.Execute(callExpress);
            Console.WriteLine(cc);
        }

        [TestMethod]
        public void ComputeAgeInQuery()
        {
            /**
             * If the computed on date is after the birthdate:
             *  - diff years and if the compute date is before the birthdate subtract 1
             *  If the computed on date is before the birthdate (negative age)
             *  - if the computed on month id greater than the birth month or equal and the computed on date is greater than the birth day subtract 1
             *  
             * 
             * */

            DateTime asOf = new DateTime(2003, 7, 3);
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                db.Database.Log = (s) => {
                    Logger.Debug(s);
                };

                var query = db.Patients.Where(p => p.BornOn.HasValue);                
                var result = query.Select(p => new
                {
                    PatientID = p.ID,
                    p.BornOn,
                    Age = (int?)((p.BornOn.Value > asOf) ?
                            (DbFunctions.DiffYears(p.BornOn.Value, asOf).Value + ((p.BornOn.Value.Month < asOf.Month || (p.BornOn.Value.Month == asOf.Month && p.BornOn.Value.Day < asOf.Day)) ? 1 : 0))
                            :
                            (DbFunctions.DiffYears(p.BornOn.Value, asOf).Value - (((p.BornOn.Value.Month > asOf.Month) || (p.BornOn.Value.Month == asOf.Month && p.BornOn.Value.Day > asOf.Day)) ? 1 : 0))
                        )
                }).Where(p => p.Age <= 1).OrderByDescending(p => p.Age);

                foreach (var pat in result)
                {
                    //Console.WriteLine(pat.PatientID + " " + pat.BornOn + " " + pat.Age);
                    Logger.Debug(string.Format("{0:yyyy-MM-dd}\t\t{1}", pat.BornOn, pat.Age));
                }
            }
        }

        [TestMethod]
        public void ComputeAgeAsExpression()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {

                var linqQuery = db.Patients.Where(p => p.ID == "10000").Select(p => new { Age = (DbFunctions.DiffYears(p.BornOn.Value, DateTime.Now).Value - (((p.BornOn.Value.Month > DateTime.Now.Month) || (p.BornOn.Value.Month == DateTime.Now.Month && p.BornOn.Value.Day > DateTime.Now.Day)) ? 1 : 0)) });

                Console.WriteLine(linqQuery.Expression);

                Console.WriteLine("Linq query result:");
                foreach (var x in linqQuery.ToArray())
                {
                    Console.WriteLine(x.Age);
                }


                var query = db.Patients.Where(p => p.ID == "10000");

                Type selectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("s", new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Type = "System.Object", Name = "Age" } });
                PropertyInfo pi = selectType.GetProperty("Age");

                //the type of object being queried
                ParameterExpression sourceItemParam = Expression.Parameter(query.ElementType, "p");

                //continue build the age calculation
                /*
                 * Age = (DbFunctions.DiffYears(p.BornOn.Value, DateTime.Now).Value - 
                 *       (
                 *          ( 
                 *              (p.BornOn.Value.Month > DateTime.Now.Month) 
                 *                  || 
                 *              ( p.BornOn.Value.Month == DateTime.Now.Month  &&  p.BornOn.Value.Day > DateTime.Now.Day)
                 *          ) ? 1 : 0)
                 *       )
                 *       
                 * Age = Current number of years minus either:
                 *              when BornOn month is greater than current datetime month or (current BornOn month is equal to current datetime month and current BornOn day is greater than current datetime day) => add 1
                 *              else add 0
                 *
                 * */

                Expression bornOnExpression = Expression.Convert(Expression.Property(sourceItemParam, "BornOn"), typeof(DateTime?));
                Expression datetimeNowExpression = Expression.Convert(Expression.Constant(DateTime.Now), typeof(DateTime?));

                Expression bornOnMonthExpression = GetInnerNullablePropertyExpression<DateTime>(bornOnExpression, "Month");
                Expression dateNowMonthExpression = GetInnerNullablePropertyExpression<DateTime>(datetimeNowExpression, "Month");

                Expression gtBornOnMonth = Expression.GreaterThan(bornOnMonthExpression, dateNowMonthExpression);
                Console.WriteLine(gtBornOnMonth);

                Expression currentMonthDayGreaterThanExpression = Expression.AndAlso(
                        Expression.Equal(bornOnMonthExpression, dateNowMonthExpression),
                        Expression.GreaterThan(GetInnerNullablePropertyExpression<DateTime>(bornOnExpression, "Day"), GetInnerNullablePropertyExpression<DateTime>(datetimeNowExpression, "Day"))
                    );
                Console.WriteLine(currentMonthDayGreaterThanExpression.ToString());

                Expression ageMod = Expression.Condition(Expression.OrElse(gtBornOnMonth, currentMonthDayGreaterThanExpression), Expression.Constant(1), Expression.Constant(0));
                Console.WriteLine(ageMod.ToString());

                //call the DbFunctions.DiffYears against the BornOn property of the source element type and the current datetime.
                Expression callDiffYears = Expression.Call(typeof(DbFunctions), "DiffYears", Type.EmptyTypes, bornOnExpression, datetimeNowExpression);
                Console.WriteLine(callDiffYears.ToString());

                Expression computeAge = Expression.Subtract(callDiffYears, Expression.Convert(ageMod, typeof(int?)));
                Console.WriteLine(computeAge.ToString());

                //bind the function call to the Age property of the select result type
                MemberBinding binding = Expression.Bind(pi, computeAge);

                Console.WriteLine(binding.ToString());

                //build the lambda that new's up the select result type with the bindings between the query type and the return type
                Expression selector = Expression.Lambda(
                        Expression.MemberInit(Expression.New(selectType.GetConstructor(Type.EmptyTypes)), new[] { binding }),
                        sourceItemParam
                    );

                Console.WriteLine(selector.ToString());

                //build the Select call that executes the selector against the query
                Expression selectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { query.ElementType, selectType }, Expression.Constant(query), selector);

                Console.WriteLine(selectCall.ToString());

                //execute the select call
                var callResult = query.Provider.CreateQuery(selectCall);
                Console.WriteLine(callResult.ToString());

                PropertyInfo agePi = selectType.GetProperty("Age");
                foreach (object v in callResult)
                {
                    Console.WriteLine(agePi.GetValue(v, null));
                }
            }
        }

        static Expression GetInnerNullablePropertyExpression<T>(Expression parentExpression, string innerPropertyName) where T : struct
        {
            Expression nullableValueExpression = Expression.Property(parentExpression, typeof(Nullable<T>).GetProperty("Value"));
            return Expression.Property(nullableValueExpression, typeof(T).GetProperty(innerPropertyName));
        }


        [TestMethod]
        public void SimpleCountAgainstDb()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                var expectedCount = db.Patients.Count();
                Console.WriteLine("Linq count:" + expectedCount);

                var methodInfos = typeof(Queryable).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Where(m => m.Name.Equals("Count") && m.GetParameters().Count() == 1).First();
                var callMI = methodInfos.MakeGenericMethod(new[] { typeof(string) });
                Console.WriteLine(callMI.ToString());

                

                Expression call = Expression.Call(callMI, Expression.Constant(db.Patients.Select(p => p.ID)));                
                Console.WriteLine(call.ToString());

                var obj = db.Patients.AsQueryable().Provider.Execute(call);
                Console.WriteLine("Expression count:" + obj);

                Assert.AreEqual(expectedCount, obj);

                var query = db.Patients.AsQueryable();
                var cc = query.Provider.Execute(Expression.Call(typeof(Queryable), "Count", new Type[] { query.ElementType }, query.Expression));
                Console.WriteLine(cc);
            }
        }

        [TestMethod]
        public void Aggregation_CountAndSelect()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {

                //var linqResults = db.Patients.Where(p => p.Sex == "M").GroupBy(p => new { p.Sex }).Select(k => new { k.Key.Sex, Total = k.Count() });
                //foreach (var r in linqResults)
                //{
                //    Console.WriteLine(r.Sex + "  " + r.Total);
                //}

                //Console.WriteLine("@@@@@ where Sex == M group by Sex @@@@@");
                //Console.WriteLine("#### direct linq ####");
                //var linqResults = db.Patients.Where(p => p.Sex == "M").GroupBy(p => new { p.Sex });
                //Console.WriteLine(linqResults);

                //create a grouping anonymous class containing property Sex
                Type groupByType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("g", new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Type = "System.Object", Name = "Sex" } });
                Type sourceType = typeof(PCORIQueryBuilder.Model.Patient);
                
                //create bindings for the group select
                ParameterExpression sourceItem = Expression.Parameter(sourceType, "p");
                IEnumerable<MemberBinding> bindings = groupByType.GetProperties().Select(prop => Expression.Bind(prop, Expression.Property(sourceItem, sourceType.GetProperty(prop.Name)))).OfType<MemberBinding>();
                LambdaExpression groupKeySelector = Expression.Lambda(
                        Expression.MemberInit(
                            Expression.New(groupByType.GetConstructor(Type.EmptyTypes)), bindings
                        ),
                        sourceItem
                    );

                IQueryable query = db.Patients.Where(p => p.Sex == "M");
                Expression call = Expression.Call(typeof(Queryable), "GroupBy", new Type[] { query.ElementType, groupKeySelector.Body.Type }, query.Expression, Expression.Quote(groupKeySelector));

                //Console.WriteLine("#### composed expression call ####");
                //var res = query.Provider.CreateQuery(call);
                //Console.WriteLine(res);

                Console.WriteLine("@@@@@ where Sex == M group by Sex @@@@@");
                Console.WriteLine("#### direct linq ####");
                var linq2 = db.Patients.Where(p => p.Sex == "M").GroupBy(p => new { p.Sex }).Select(k => new { k.Key.Sex, Count = k.Count() });
                Console.WriteLine(linq2.Expression);
                Console.WriteLine(linq2);

                query = query.Provider.CreateQuery(call);

                //create the return type
                Type selectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("s", new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Type = "System.Object", Name = "Sex" }, new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Type = "System.Object", Name = "Count" } });
                //the current type returned by the query will be an IGrouping of grouping key type, and root query type

                //need to apply the bindings between the select type and the IGrouping as applicable

                //does a recursive on creating Property expression for each property up the change, uses the bound property type
                ParameterExpression sourceItem2 = Expression.Parameter(query.ElementType, "k");
                IEnumerable<MemberBinding> selectBindings = selectType.GetProperties().Select(prop => Expression.Bind(prop, CreatePropertyBinding(prop, query, sourceItem2))).OfType<MemberBinding>();

                Expression selectLambda = Expression.Lambda(Expression.MemberInit(
                        Expression.New(selectType), selectBindings
                    ), sourceItem2);

                Expression callSelect = Expression.Call(typeof(Queryable), "Select", new Type[] { query.ElementType, selectType }, query.Expression, Expression.Quote(selectLambda));

                Console.WriteLine("#### composed expression call with select ####");
                var res = query.Provider.CreateQuery(callSelect);
                Console.WriteLine(res.Expression);
                Console.WriteLine(res);
            }

        }
        
        [TestMethod]
        public void StratifyByHeight_2InchGroups()
        {
            //get patient count stratified by 2" groupings where height <= 36"

            /**
             * When working with tables that could include an Encounter reference:
             * Always use the most recent based on the encounter associated.
             * The encounter could be null, if no observation period use the most recent based on date of table
             * 
             * IE for the above example since there is no observation period specified use the most recent by Measure_Date on the Vitals table
             * 
             * */

            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                //var query = db.Patients.Where(p => db.Vitals.Any(v => v.PatientID == p.ID && v.Height <= 36))
                //                       .Select(p => new StratifyResult
                //                            {
                //                                ID = p.ID,
                //                                StratificationValue = db.Vitals.Where(v => v.PatientID == p.ID && v.Height <= 36).OrderByDescending(v => v.MeasuredOn).Select(v => Math.Floor(v.Height.Value / 2)).FirstOrDefault(),
                //                            });
                //Console.WriteLine(query.Expression.ToString());

                //var grouped = query.GroupBy(s => new { s.StratificationValue }).Select(s => new { StratificationValue = s.Key.StratificationValue, Count = s.Count() });
                //Console.WriteLine(grouped.Expression);
                //Console.WriteLine(grouped.ToString());

                //The above as a single query
                var query = db.Patients.Where(p => db.Vitals.Any(v => v.PatientID == p.ID && v.Height <= 36))
                                       .Select(p => new StratifyResult
                                       {
                                           PatientID = p.ID,
                                           StratificationValue = db.Vitals.Where(v => v.PatientID == p.ID && v.Height <= 36).OrderByDescending(v => v.MeasuredOn).Select(v => Math.Floor(v.Height.Value / 2)).FirstOrDefault(),
                                       })
                                       .GroupBy(s => new { s.StratificationValue })
                                       .Select(s => new { StratifiationValue = s.Key.StratificationValue, Count = s.Count() });

                //NOTE: cannot do unions with the stratification values as part of db query due to EF limitation, will have to do that in memory after

                ParameterExpression patientParameter = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Patient), "pp");

                var rootVitalsSelect = db.Vitals.Where(v => v.Height <= 36).OrderByDescending(v => v.MeasuredOn).AsQueryable();

                ParameterExpression rvQueryParam = Expression.Parameter(rootVitalsSelect.ElementType, "v");
                Expression rvPatientIDPropertyExpression = Expression.Property(rvQueryParam, "PatientID");

                //patientParameter will likely need to be the same one use for binding to the select values from the root query object
                
                BinaryExpression rvPatientIDEqualsExpression = Expression.Equal(rvPatientIDPropertyExpression, Expression.Property(patientParameter, "ID"));                

                LambdaExpression labmda = Expression.Lambda(rvPatientIDEqualsExpression, rvQueryParam);
                //Console.WriteLine(labmda.ToString());                
                
                Expression callWhere = Expression.Call(typeof(Queryable), "Where", new Type[]{rootVitalsSelect.ElementType}, rootVitalsSelect.Expression, Expression.Quote(labmda));
                //Console.WriteLine(callWhere.ToString());

                rootVitalsSelect = ApplyExpression(rootVitalsSelect, callWhere);

                //Console.WriteLine(rootVitalsSelect.Expression.ToString());

                //add the select

                //create Expression.Property(parameter expression of vital query, property info of height)
                //create lambda(expression property, parameter expression of vital query)

                Expression innerHeight = Expression.Property(Expression.Property(rvQueryParam, "Height"), "Value");
                Expression divideExpression = Expression.Divide(innerHeight, Expression.Constant(2d));
                Expression floorInnerHeightStrat = Expression.Call(typeof(Math), "Floor", Type.EmptyTypes, divideExpression);
                LambdaExpression selectLambda = Expression.Lambda(floorInnerHeightStrat, rvQueryParam);

                Expression selectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { rootVitalsSelect.ElementType, selectLambda.Body.Type }, rootVitalsSelect.Expression, Expression.Quote(selectLambda));
                //Console.WriteLine(selectCall.ToString());

                Expression firstOrDefaultCall = Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[]{ typeof(double) }, selectCall);
                //Console.WriteLine(firstOrDefaultCall.ToString());


                //create the base query and select
                var baseQuery = db.Patients.Where(p => db.Vitals.Any(v => v.PatientID == p.ID && v.Height <= 36));

                Type selectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("Pat", new []{ new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Type = "System.Object", Name = "PatientID" }, new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Type = "System.Object", Name = "StratificationValue" }});
                IEnumerable<MemberBinding> selectBindings = new[] {
                    Expression.Bind(selectType.GetProperty("PatientID"), Expression.Property(patientParameter, patientParameter.Type.GetProperty("ID"))),
                    Expression.Bind(selectType.GetProperty("StratificationValue"), firstOrDefaultCall)
                };

                LambdaExpression baseSelectLam = Expression.Lambda(Expression.MemberInit(
                        Expression.New(selectType), selectBindings
                    ), patientParameter);

                Expression baseSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { baseQuery.ElementType, selectType }, baseQuery.Expression, Expression.Quote(baseSelectLam));
                //Console.WriteLine(baseSelectCall);

                //group the query on the stratification value
                Type groupSelectType = TypeBuilderHelper.CreateType("g", "StratificationValue");
                ParameterExpression gSourceItem = Expression.Parameter(selectType, "sg");
                LambdaExpression groupKeySelector = Expression.Lambda(
                        Expression.MemberInit(
                            Expression.New(groupSelectType),
                            new[] { Expression.Bind(groupSelectType.GetProperty("StratificationValue"), Expression.Property(gSourceItem, gSourceItem.Type.GetProperty("StratificationValue"))) }
                        ),
                        gSourceItem
                    );

                MethodCallExpression groupCall = Expression.Call(typeof(Queryable), "GroupBy", new Type[] { selectType, groupKeySelector.Body.Type }, baseSelectCall, Expression.Quote(groupKeySelector));
                //Console.WriteLine(groupCall.ToString());

                //select the from the grouping
                Type finalSelectType = TypeBuilderHelper.CreateType("f", "StratificationValue", "Count");

                //the type of groupCall is IQueryable<IGrouping<>>, we need the IGrouping type for that parameter
                Type IGroupingType = GetGenericType(typeof(IQueryable<>), groupCall.Type).GetGenericArguments().First();
                ParameterExpression iGroupingParamExpression = Expression.Parameter(IGroupingType, "k");
                LambdaExpression finalSelectLam = Expression.Lambda(
                        Expression.MemberInit( Expression.New(finalSelectType), new []{
                            Expression.Bind(finalSelectType.GetProperty("StratificationValue"), CreateMemberExpressionForIGrouping(iGroupingParamExpression, "StratificationValue")),
                            Expression.Bind(finalSelectType.GetProperty("Count"), CreateCountExpression(groupCall.Type, iGroupingParamExpression))
                        }),
                        iGroupingParamExpression
                    );

                MethodCallExpression finalSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { IGroupingType, finalSelectType }, groupCall, Expression.Quote(finalSelectLam));
                Console.WriteLine(finalSelectCall.ToString());

                var res = baseQuery.Provider.CreateQuery(finalSelectCall);
                PropertyInfo prop1 = finalSelectType.GetProperty("StratificationValue");
                PropertyInfo prop2 = finalSelectType.GetProperty("Count");
                foreach (object r in res)
                {
                    Console.WriteLine("{0}: {1}", prop1.GetValue(r, null), prop2.GetValue(r, null));
                }
            }

        }


        [TestMethod]
        public void StratifyByWeight_StratificationReturnedAsTextBasedOnStratValue()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {

                var query = db.Patients.Where(p => p.Vitals.Any(v => v.Weight.HasValue))
                                       .Select(p => 
                                           new { 
                                               PatientID = p.ID,
                                               Weight = Math.Floor(p.Vitals.Where(v => v.Weight.HasValue).OrderByDescending(v => v.MeasuredOn).Select(v => v.Weight.Value).FirstOrDefault() / 5d)
                                               //Weight = Math.Floor(db.Vitals.Where(v => v.PatientID == p.ID && v.Weight.HasValue).OrderByDescending(v => v.MeasuredOn).Select(v => v.Weight.Value).FirstOrDefault()/5d)
                                       });

                Console.WriteLine(query.Expression);
                throw new Exception();

                var groupedQuery = query.GroupBy(k => new { Weight = k.Weight });
                var groupSelect = groupedQuery
                                        .OrderBy(k => k.Key.Weight)
                                        .Select(g => new { Weight = string.Concat((g.Key.Weight * 5).ToString(), " - ", (g.Key.Weight * 5 + 5).ToString()), Count = g.Count() });
                                        //.Select(g => new { Weight = g.Key.Weight * 5 + " - " + (g.Key.Weight * 5 + 5), Count = g.Count() });

                //Console.WriteLine(groupSelect.Expression.ToString());

                //foreach (var p in groupSelect)
                //{
                //    Console.WriteLine(p.Weight + ": " + p.Count);
                //}

                var baseQuery = db.Patients.Where(p => p.Vitals.Any(v => v.Weight.HasValue));

                /*** weight select ***/
                ParameterExpression pe_sourceQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Patient), "p");//Note: when implemented, type should be the source query element type
                ParameterExpression pe_vitalsQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Vital), "v");

                BinaryExpression be_vitalsPatient = Expression.Equal(Expression.Property(pe_vitalsQueryType, "PatientID"), Expression.Property(pe_sourceQueryType, "ID"));
                BinaryExpression be_vitalsWeightNotNull = Expression.Equal(Expression.Property(Expression.Property(pe_vitalsQueryType, "Weight"), "HasValue"), Expression.Constant(true, typeof(bool)));

                //MethodCallExpression weightWhere = Expression.Call(typeof(Queryable), "Where", new Type[] { pe_vitalsQueryType.Type }, db.Vitals.AsQueryable().Expression, Expression.Quote(Expression.Lambda(Expression.AndAlso(be_vitalsPatient, be_vitalsWeightNotNull), pe_vitalsQueryType)));
                MethodCallExpression weightWhere = Expression.Call(typeof(Enumerable), "Where", new Type[] { pe_vitalsQueryType.Type }, db.Vitals.AsQueryable().Expression, Expression.Quote(Expression.Lambda(Expression.AndAlso(be_vitalsPatient, be_vitalsWeightNotNull), pe_vitalsQueryType)));
                
                Expression measureOnSelector = Expression.Property(pe_vitalsQueryType, "MeasuredOn");
                MethodCallExpression orderByMeasureOn = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { pe_vitalsQueryType.Type, measureOnSelector.Type }, weightWhere, Expression.Quote(Expression.Lambda(measureOnSelector, pe_vitalsQueryType)));

                Expression weightSelector = Expression.Property(Expression.Property(pe_vitalsQueryType, "Weight"), "Value");
                MethodCallExpression weightSelect = Expression.Call(typeof(Queryable), "Select", new Type[] { pe_vitalsQueryType.Type, weightSelector.Type }, orderByMeasureOn, Expression.Quote(Expression.Lambda(weightSelector, pe_vitalsQueryType)));

                MethodCallExpression firstOrDefaultVitalWeight = Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[] { typeof(double) }, weightSelect);

                Expression divideExpression = Expression.Divide(firstOrDefaultVitalWeight, Expression.Constant(5d));
                Expression weightFloor = Expression.Call(typeof(Math), "Floor", Type.EmptyTypes, divideExpression);

                Type vitalsSelectType = TypeBuilderHelper.CreateType("s", "PatientID", "Weight");
                IEnumerable<MemberBinding> innerSelectBindings = new[] {
                    Expression.Bind(vitalsSelectType.GetProperty("PatientID"), Expression.Property(pe_sourceQueryType, "ID")),
                    Expression.Bind(vitalsSelectType.GetProperty("Weight"), weightFloor)
                };

                LambdaExpression innerSelectLambda = Expression.Lambda(Expression.MemberInit(Expression.New(vitalsSelectType), innerSelectBindings), pe_sourceQueryType);
                MethodCallExpression innerSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { pe_sourceQueryType.Type, vitalsSelectType }, baseQuery.Expression, Expression.Quote(innerSelectLambda));

                //group by weight stratification value
                Type groupSelectType = TypeBuilderHelper.CreateType("g", "Weight");
                ParameterExpression pe_innerSelectType = Expression.Parameter(vitalsSelectType, "v");
                LambdaExpression groupKeySelector = Expression.Lambda(Expression.MemberInit(
                        Expression.New(groupSelectType),
                        new[] { Expression.Bind(groupSelectType.GetProperty("Weight"), Expression.Property(pe_innerSelectType, pe_innerSelectType.Type.GetProperty("Weight"))) }
                    ), pe_innerSelectType);

                MethodCallExpression groupCall = Expression.Call(typeof(Queryable), "GroupBy", new Type[] { vitalsSelectType, groupSelectType }, innerSelectCall, Expression.Quote(groupKeySelector));

                Type groupCallType = GetGenericType(typeof(IQueryable<>), groupCall.Type).GetGenericArguments().First();
                ParameterExpression pe_groupCallType = Expression.Parameter(groupCallType, "k");
                Type groupCallTypeElementType = groupCallType.GetGenericArguments().First();
                
                //order by the group key Weight
                Expression groupKeyWeightSelector = Expression.Property(Expression.Property(pe_groupCallType, "Key"), "Weight");
                MethodCallExpression orderByGroupKeyWeight = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { groupCallType, groupKeyWeightSelector.Type }, groupCall, Expression.Quote(Expression.Lambda(groupKeyWeightSelector, pe_groupCallType)));

                //build the final select
                Type finalSelectType = TypeBuilderHelper.CreateType("ss", "Weight", "Count");

                //convert the weight value into a text string describing the stratification
                Expression weightString1 = Expression.Call(Expression.Multiply(Expression.Convert(groupKeyWeightSelector, typeof(double)), Expression.Constant(5d)), typeof(double).GetMethod("ToString", Type.EmptyTypes));
                Expression weightStringJoin = Expression.Constant(" - ", typeof(string));
                Expression weightString2 = Expression.Call( Expression.Add(Expression.Multiply(Expression.Convert(groupKeyWeightSelector, typeof(double)), Expression.Constant(5d)), Expression.Constant(5d)), typeof(double).GetMethod("ToString", Type.EmptyTypes));

                MethodInfo mi = typeof(string).GetMethod("Concat", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(string), typeof(string), typeof(string) }, null);
                Expression weightString = Expression.Call(mi, weightString1, weightStringJoin, weightString2);

                IEnumerable<MemberBinding> finalSelectBindings = new[]{
                    Expression.Bind(finalSelectType.GetProperty("Weight"), weightString),
                    Expression.Bind(finalSelectType.GetProperty("Count"), CreateCountExpression(orderByGroupKeyWeight.Type, pe_groupCallType))
                };

                MethodCallExpression finalSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { groupCallType, finalSelectType }, orderByGroupKeyWeight, Expression.Quote(Expression.Lambda(Expression.MemberInit(Expression.New(finalSelectType), finalSelectBindings), pe_groupCallType)));

                Console.WriteLine(finalSelectCall.ToString());

                var result = baseQuery.Provider.CreateQuery(finalSelectCall);
                PropertyInfo pi1 = finalSelectType.GetProperty("Weight");
                PropertyInfo pi2 = finalSelectType.GetProperty("Count");
                foreach (var r in result)
                {
                    Console.WriteLine("{0}: {1}", pi1.GetValue(r, null), pi2.GetValue(r, null));
                }
                
            }


        }

        [TestMethod]
        public void CombineStrings()
        {
            MethodInfo mi = typeof(string).GetMethod("Concat", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(string), typeof(string), typeof(string) }, null);
            Expression concat = Expression.Call(mi, Expression.Constant("Part", typeof(string)), Expression.Constant(" - Part 2", typeof(string)), Expression.Constant(" - Part3", typeof(string)));
            Console.WriteLine(concat.ToString());
        }

        [TestMethod]
        public void QueryForEncountAdmittedOnSubSelect()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                DateTime startRange = DateTime.Parse("2008-01-01");
                DateTime endRange = DateTime.Parse("2011-01-01");
                var query = db.Patients.Where(p => p.Encounters.Any(e => e.AdmittedOn >= startRange && e.AdmittedOn <= endRange ));

                ParameterExpression pe_sourceQueryType = Expression.Parameter(query.ElementType, "p"); ;
                Expression patientIDProp = Expression.Property(pe_sourceQueryType, pe_sourceQueryType.Type.GetProperty("ID"));
                Expression encountersProp = Expressions.AsQueryable(Expression.Property(pe_sourceQueryType, "Encounters"));

                ParameterExpression pe_encountersQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Encounter), "enc");

                Expression admittedOnSelector = Expression.Property(pe_encountersQueryType, "AdmittedOn");

                BinaryExpression be_encounterPatient = Expression.Equal(Expression.Property(pe_sourceQueryType, "ID"), Expression.Property(pe_encountersQueryType, "PatientID"));
                
                BinaryExpression predicate = be_encounterPatient;
                predicate = Expression.AndAlso(predicate, Expression.GreaterThanOrEqual(admittedOnSelector, Expression.Constant(startRange.Date)));
                predicate = Expression.AndAlso(predicate, Expression.LessThanOrEqual(admittedOnSelector, Expression.Constant(endRange.Date)));

                MethodCallExpression admittedOnWhere = Expressions.Where(encountersProp, Expression.Lambda(predicate, pe_encountersQueryType));

                MethodCallExpression orderByAdmittedOn = Expressions.OrderByAscending(admittedOnWhere, Expression.Lambda(admittedOnSelector, pe_encountersQueryType));

                MethodCallExpression admittedOnSelect = Expression.Call(typeof(Queryable), "Select", new Type[] { pe_encountersQueryType.Type, typeof(DateTime?) }, orderByAdmittedOn, Expression.Quote(Expression.Lambda(Expression.Convert( admittedOnSelector, typeof(DateTime?)), pe_encountersQueryType)));

                MethodCallExpression firstOrDefaultAdmittedOn = Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[] { typeof(DateTime?) }, admittedOnSelect);

                //stratify by months
                //Expression stratificationModifier = Expression.Property(Expressions.DbFunctionsCreateTruncatedDate(Expression.Convert(Expression.Property(firstOrDefaultAdmittedOn, "Year"), typeof(int?)), Expression.Convert(Expression.Property(firstOrDefaultAdmittedOn, "Month"), typeof(int?))), "Value");

                Expression prop = Expression.Property(firstOrDefaultAdmittedOn, "Value");
                Expression yearPartString = Expressions.CallToString<int>(Expression.Property(prop, "Year"));
                Expression monthPartString = Expressions.CallToString<int>(Expression.Property(prop, "Month"));
                prop = Expressions.ConcatStrings(yearPartString, Expression.Constant("-"), monthPartString);

                Expression stratificationModifier = Expression.Condition(Expression.NotEqual(firstOrDefaultAdmittedOn, Expression.Constant(null)), prop, Expression.Constant("", typeof(string)));
            

                Type innerSelectType = TypeBuilderHelper.CreateType("ss", "PatientID", "AdmittedOn");
                MemberBinding[] bindings = new[] {
                    Expression.Bind(innerSelectType.GetProperty("PatientID"), patientIDProp),
                    Expression.Bind(innerSelectType.GetProperty("AdmittedOn"), stratificationModifier)
                };

                LambdaExpression innerSelector = Expression.Lambda(Expression.MemberInit(Expression.New(innerSelectType), bindings), pe_sourceQueryType);
                MethodCallExpression innerSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { query.ElementType, innerSelectType }, query.Expression, Expression.Quote(innerSelector));


                //group on admitted on
                Type groupKeyType = TypeBuilderHelper.CreateType("g", "AdmittedOn");
                ParameterExpression pe_innerSelectType = Expression.Parameter(innerSelectType, "k");
                MemberBinding[] groupKeyBindings = new[] { Expression.Bind(groupKeyType.GetProperty("AdmittedOn"), Expression.Property(pe_innerSelectType, pe_innerSelectType.Type.GetProperty("AdmittedOn"))) };
                LambdaExpression groupKeySelect = Expression.Lambda(Expression.MemberInit(Expression.New(groupKeyType), groupKeyBindings), pe_innerSelectType);
                MethodCallExpression groupCall = Expression.Call(typeof(Queryable), "GroupBy", new Type[] { innerSelectType, groupKeySelect.Body.Type }, innerSelectCall, Expression.Quote(groupKeySelect));
                

                Type groupingType = Lpp.Objects.Dynamic.Expressions.GetGenericType(typeof(IQueryable<>), groupCall.Type).GetGenericArguments().First();
                ParameterExpression groupKeyParameterExpr = Expression.Parameter(groupingType, "k");

                Expression pe_innerSelect = Expression.Parameter(innerSelectType, "k");

                Type finalSelectType = TypeBuilderHelper.CreateType("ss", "AdmittedOn", "Count");
                MemberBinding[] finalSelectBindings = new[]{
                    Expression.Bind(finalSelectType.GetProperty("AdmittedOn"), Expressions.ChildPropertyExpression(groupKeyParameterExpr, "Key", "AdmittedOn")),
                    Expression.Bind(finalSelectType.GetProperty("Count"), Expressions.Count(groupKeyParameterExpr))
                };

                LambdaExpression finalSelector = Expression.Lambda(Expression.MemberInit(Expression.New(finalSelectType), finalSelectBindings), groupKeyParameterExpr);
                MethodCallExpression finalSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { groupingType, finalSelectType }, groupCall, Expression.Quote(finalSelector));


                var res = query.Provider.CreateQuery(finalSelectCall);
                Console.WriteLine(res.ToString());
            }
        }

        [TestMethod]
        public void OracleAgeStratificationInLINQ()
        {
            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, OracleConnectionString, true))
            {
                DateTime now = DateTime.Now.Date;
                var query = db.Patients
                    .Where(p => p.BornOn.HasValue && DbFunctions.DiffYears(p.BornOn, now).Value - (((p.BornOn.Value.Month > now.Month) || (p.BornOn.Value.Month == now.Month && p.BornOn.Value.Day > now.Day)) ? 1 : 0) >= 10 && DbFunctions.DiffYears(p.BornOn, now).Value - (((p.BornOn.Value.Month > now.Month) || (p.BornOn.Value.Month == now.Month && p.BornOn.Value.Day > now.Day)) ? 1 : 0) <= 76)
                    .Select(p => (DbFunctions.DiffYears(p.BornOn, now).Value - (((p.BornOn.Value.Month > now.Month) || (p.BornOn.Value.Month == now.Month && p.BornOn.Value.Day > now.Day)) ? 1 : 0)).ToString());

                foreach (var d in query)
                {
                    Console.WriteLine(d);
                }
            }
        }

        [TestMethod]
        public void AgeRangeWhereAgeIsWithinCriteriaGroup()
        {
            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.SQLServer, ConnectionString))
            {
                var query = from p in db.Patients
                            where p.Sex == "M"
                            && p.Vitals.Any(v => v.Height >= 36 && v.Height <= 66
                            && (DbFunctions.DiffYears(p.BornOn, v.Encounter.AdmittedOn).Value - (((p.BornOn.Value.Month > v.Encounter.AdmittedOn.Month) || (p.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && p.BornOn.Value.Day > v.Encounter.AdmittedOn.Day)) ? 1 : 0)) >= 5
                            && (DbFunctions.DiffYears(p.BornOn, v.Encounter.AdmittedOn).Value - (((p.BornOn.Value.Month > v.Encounter.AdmittedOn.Month) || (p.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && p.BornOn.Value.Day > v.Encounter.AdmittedOn.Day)) ? 1 : 0)) <= 25
                            )
                            select new
                            {
                                PatientID = p.ID,
                                Sex = p.Sex,
                                Age = (p.Encounters.Where(e => e.Vitals.Any(vv => vv.Weight >= 36 && vv.Weight <= 66)
                                            && (DbFunctions.DiffYears(p.BornOn, e.AdmittedOn).Value - (((p.BornOn.Value.Month > e.AdmittedOn.Month) || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)) >= 5
                                            && (DbFunctions.DiffYears(p.BornOn, e.AdmittedOn).Value - (((p.BornOn.Value.Month > e.AdmittedOn.Month) || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)) <= 25
                                        )
                                        .OrderBy(e => e.AdmittedOn)
                                        .Select(e => (DbFunctions.DiffYears(p.BornOn, e.AdmittedOn).Value - (((p.BornOn.Value.Month > e.AdmittedOn.Month) || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0))).FirstOrDefault()
                                    )
                            };


                Console.WriteLine(query.ToString());
                Console.WriteLine("");
                Console.WriteLine(query.Expression.ToString());


                foreach (var result in query.GroupBy(k => new { k.Sex, k.Age }).ToArray())
                {
                    Console.WriteLine("{0} {1} {2}", result.Key.Sex, result.Key.Age, result.Count());
                }

            }
        }

        [TestMethod]
        public void OracleFailsTranslatingNumberToString()
        {
            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, OracleConnectionString))
            {
                var rootQuery = db.Patients.Where(p => p.Sex == "M" && p.Vitals.Any(v => v.Height >= 36 && v.Height <= 66));

                ParameterExpression sourceTypeParameter = Expression.Parameter(rootQuery.ElementType, "p");

                Type innerSelectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("s", new[]{
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO {
                        Name = "ID",
                        As = "PatientID",
                        Type = "System.String",
                        Aggregate = "Count"
                    },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Height",
                        Type = typeof(double?).FullName
                    }
                });

                //create the inner select height binding
                ParameterExpression pe_vitalsQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Vital), "v");
                Expression heightSelector = Expression.Property(pe_vitalsQueryType, "Height");
                Expression heightSelector_Value = Expression.Property(heightSelector, "Value");

                BinaryExpression be_vitalsPatient = Expression.Equal(Expression.Property(pe_vitalsQueryType, "PatientID"), Expression.Property(sourceTypeParameter, "ID"));
                BinaryExpression be_vitalsHeightNotNull = Expression.Equal(Expression.Property(Expression.Property(pe_vitalsQueryType, "Height"), "HasValue"), Expression.Constant(true, typeof(bool)));

                var ex1 = Expression.GreaterThanOrEqual(heightSelector_Value, Expression.Constant(36d, typeof(double)));
                var ex2 = Expression.LessThanOrEqual(heightSelector_Value, Expression.Constant(66d, typeof(double)));

                Expression selectHeightPredicate = Expression.AndAlso(be_vitalsHeightNotNull, Expression.AndAlso(ex1, ex2));

                Expression vitalsProp = Expressions.AsQueryable(Expression.Property(sourceTypeParameter, "Vitals"));
                MethodCallExpression heightWhere = Expressions.Where(vitalsProp, Expression.Lambda(selectHeightPredicate, pe_vitalsQueryType));

                Expression measureOnSelector = Expression.Property(pe_vitalsQueryType, "MeasuredOn");
                MethodCallExpression orderByMeasureOn = Expressions.OrderByAscending(heightWhere, Expression.Lambda(measureOnSelector, pe_vitalsQueryType));

                MethodCallExpression heightSelect = Expression.Call(typeof(Queryable), "Select", new Type[] { pe_vitalsQueryType.Type, heightSelector.Type }, orderByMeasureOn, Expression.Quote(Expression.Lambda(heightSelector, pe_vitalsQueryType)));

                MethodCallExpression firstOrDefaultVitalHeight = Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[] { typeof(double?) }, heightSelect);

                ////binding without convert for stratification
                //MemberBinding isHeightBinding = Expression.Bind(innerSelectType.GetProperty("Height"), firstOrDefaultVitalHeight);

                DTO.Enums.HeightStratification stratification = DTO.Enums.HeightStratification.FourInch;
                var c1 = Expression.Equal(Expression.Property(firstOrDefaultVitalHeight, "HasValue"), Expression.Constant(true));
                var c2 = Expressions.MathFloor<double?>(Expression.Divide(Expression.Property(firstOrDefaultVitalHeight, "Value"), Expression.Constant(new Nullable<double>(Convert.ToDouble((int)stratification)))));
                var c3 = Expression.Convert(Expression.Constant(null), typeof(double?));
                Expression stratificationExp = Expression.Condition(c1, c2, c3);

                MemberBinding isHeightBinding = Expression.Bind(innerSelectType.GetProperty("Height"), stratificationExp);

                //create the inner select PatientID binding
                MemberBinding isPatientBinding = Expression.Bind(innerSelectType.GetProperty("PatientID"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ID")));

                //apply the inner select
                LambdaExpression innerSelector = Expression.Lambda(Expression.MemberInit(Expression.New(innerSelectType), isPatientBinding, isHeightBinding), sourceTypeParameter);
                MethodCallExpression innerSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { rootQuery.ElementType, innerSelectType }, rootQuery.Expression, Expression.Quote(innerSelector));

                Expression queryExpression = innerSelectCall;


                //apply the stratification: group by height and get patient count
                Type groupingKeyType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("g", new []{
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Height",
                        Type = typeof(double?).FullName
                    }
                });

                //get the group key bindings
                ParameterExpression innerSelectParameterExpr = Expression.Parameter(innerSelectType, "k");
                IEnumerable<MemberBinding> groupKeySelectBindings = new[] { Expression.Bind(groupingKeyType.GetProperty("Height"), Expression.Property(innerSelectParameterExpr, innerSelectParameterExpr.Type.GetProperty("Height"))) };

                //call the grouping on the key
                LambdaExpression groupKeySelector = Expression.Lambda(Expression.MemberInit(Expression.New(groupingKeyType), groupKeySelectBindings), innerSelectParameterExpr);
                MethodCallExpression groupCall = Expression.Call(typeof(Queryable), "GroupBy", new Type[] { innerSelectType, groupKeySelector.Body.Type }, innerSelectCall, Expression.Quote(groupKeySelector));

                //build the final select type
                Type finalSelectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("ss", new []{
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO {
                        Name = "PatientID",
                        As = "Patients",
                        Type = "System.Int32",
                        Aggregate = "Sum"
                    },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "Height",
                        Type = typeof(object).FullName
                    }
                });

                //get the final select type bindings
                Type groupingType = Lpp.Objects.Dynamic.Expressions.GetGenericType(typeof(IQueryable<>), groupCall.Type).GetGenericArguments().First();
                //TODO: we know we want an IGrouping<innerSelectType>, can we just go there directly and avoid going into the groupCal query type?

                ParameterExpression groupKeyParameterExpr = Expression.Parameter(groupingType, "k");

                MemberBinding finalPatientIDBinding = Expression.Bind(finalSelectType.GetProperty("Patients"), Expressions.Count(groupKeyParameterExpr));

                //convert the stratification value for height to a string that describes the stratification
                //double stratificationValue = Convert.ToDouble((int)stratification);
                Expression prop = Expressions.ChildPropertyExpression(groupKeyParameterExpr, "Key", "Height");
                //Expression fygString1 = Expressions.CallToString<double>(Expression.Multiply(Expression.Property(prop, "Value"), Expression.Constant(stratificationValue)));
                //Expression fygStringJoin = Expression.Constant(" - ", typeof(string));
                //Expression fygString2 = Expressions.CallToString<double>(Expression.Add(Expression.Multiply(Expression.Property(prop, "Value"), Expression.Constant(stratificationValue)), Expression.Constant(stratificationValue - 1d)));

                //prop = Expression.Condition(Expression.Equal(prop, Expression.Constant(null)), Expression.Convert(Expression.Constant(null), typeof(string)), Expressions.ConcatStrings(fygString1, fygStringJoin, fygString2));

                //prop = Expressions.CallToString<double>(Expression.Property(prop, "Value"));
                //prop = Expressions.ConvertToDoubleToString(Expression.Property(prop, "Value"));
                prop = Expression.Convert(Expression.Property(prop, "Value"), typeof(string));
                

                MemberBinding finalHeightBinding = Expression.Bind(finalSelectType.GetProperty("Height"), prop);


                IEnumerable<MemberBinding> finalSelectBindings = new []{ finalPatientIDBinding, finalHeightBinding };

                //call the final select
                LambdaExpression finalSelector = Expression.Lambda(Expression.MemberInit(Expression.New(finalSelectType), finalSelectBindings), groupKeyParameterExpr);
                MethodCallExpression finalSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { groupingType, finalSelectType }, groupCall, Expression.Quote(finalSelector));
                
                queryExpression = finalSelectCall;

                Logger.Debug(queryExpression.ToString());

                IQueryable query = rootQuery.Provider.CreateQuery(queryExpression);
                Logger.Debug(query.ToString());

                Logger.Debug("Executing query:");

                StringBuilder sb = new StringBuilder();
                var patientIDPropInfo = innerSelectType.GetProperty("PatientID");
                var heightPropInfo = innerSelectType.GetProperty("Height");
                foreach (var item in query)
                {
                    sb.AppendLine(patientIDPropInfo.GetValue(item) + ",\t" + heightPropInfo.GetValue(item));
                }

                Logger.Debug(sb.ToString());
            }
        }

        public class OracleConvert
        {
            [MaxLength(200)]
            public string Height { get; set; }
        }

        [TestMethod]
        public void OracleFailsTranslatingNumberToString2()
        {
            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, OracleConnectionString))
            {

                //var grouped = db.Patients.Where(p => p.Vitals.Any(v => v.Height >= 36d && v.Height <= 66d))
                //                       .Select(p => new { 
                //                            PatientID = p.ID,
                //                            //Height = Math.Floor((p.Vitals.Where(v => v.Height >= 36d && v.Height <= 66d).OrderBy(v => v.MeasuredOn).Select(v =>v.Height).FirstOrDefault().Value / 4d)).ToString()
                //                            //Height = (p.Vitals.Where(v => v.Height >= 36d && v.Height <= 66d).OrderBy(v => v.MeasuredOn).Select(v =>v.Height).FirstOrDefault().Value / 4d).ToString()
                //                            //Height = (p.Vitals.Where(v => v.Height >= 36d && v.Height <= 66d).OrderBy(v => v.MeasuredOn).Select(v =>v.Height.Value).FirstOrDefault()).ToString()
                //                            Height = (p.Vitals.Where(v => v.Height.HasValue && v.Height >= 36d && v.Height <= 66d).OrderBy(v => v.MeasuredOn).Select(v =>v.Height.Value).FirstOrDefault()).ToString().Trim()
                //                       });

                ////var grouped = query.GroupBy(k => new { Height = k.Height })
                ////                    .Select(k => new
                ////                    {
                ////                        Height = string.Concat( (k.Key.Height * 4d).ToString(), " - ", ( (k.Key.Height * 4d) + 3 ).ToString()),
                ////                        Count = k.Count()
                ////                    });



                var grouped = db.Vitals.Where(v => v.Height.HasValue && v.Height >= 36d && v.Height <= 66d).Select(v => new { Height = v.Height.Value.ToString() + " inches" }).Take(50);

                Logger.Debug(grouped.Expression.ToString());
                Logger.Debug(grouped.ToString());


                foreach (var item in grouped.ToArray())
                {
                    Type it = item.GetType();
                    List<string> line = new List<string>();
                    foreach (var prp in it.GetProperties())
                    {
                        line.Add(prp.GetValue(item).ToString());
                    }

                    Logger.Debug(string.Join(",\t", line));
                }

            }
        }

        public class OracleDateTimeFormatProvider : IFormatProvider
        {
            public object GetFormat(Type formatType)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void OracleFailsFormatingDateToString()
        {
            
            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, OracleConnectionString))
            {
                var query = db.Encounters.Select(enc => new { AdmittedOn = DbFunctions.CreateDateTime(enc.AdmittedOn.Year, enc.AdmittedOn.Month, 1, 0, 0, 0d) }).Take(50);
                //var query = db.Encounters.Select(enc => new { AdmittedOn = enc.AdmittedOn, AYear = enc.AdmittedOn.Year, AMonth = enc.AdmittedOn.Month }).Take(50);

                Logger.Debug(query.Expression.ToString());
                Logger.Debug(query.ToString());

                var results = query.ToArray();
                foreach (var item in results)
                {
                    Type it = item.GetType();
                    List<string> line = new List<string>();
                    foreach (var prp in it.GetProperties())
                    {
                        line.Add(prp.GetValue(item).ToString());
                    }

                    Logger.Debug(string.Join(",\t", line));
                }
            }
        }

        static IQueryable<T> ApplyExpression<T>(IQueryable<T> source, Expression expr)
        {
            return source.Provider.CreateQuery<T>(expr);
        }

        static MethodCallExpression CreateCountExpression(Type sourceType, ParameterExpression sourceTypeParameter)
        {
            Type enumerableType = GetGenericType(typeof(IEnumerable<>), sourceType.GetGenericArguments().First());
            Type elementType = enumerableType.GetGenericArguments().First();

            MethodCallExpression countExpression = Expression.Call(typeof(Enumerable), "Count", new Type[] { elementType }, sourceTypeParameter);
            return countExpression;
        }

        static MemberExpression CreateMemberExpressionForIGrouping(ParameterExpression source, string innerProperty)
        {   
            //this assumes the source is IGrouping<>
            MemberExpression keyProp = Expression.Property(source, source.Type.GetProperty("Key"));
            //get the inner property from the key object
            MemberExpression innerProp = Expression.Property(keyProp, keyProp.Type.GetProperty(innerProperty));

            return innerProp;
        }


        class StratifyResult
        {
            public string PatientID { get; set; }
            public double StratificationValue { get; set; }
            public string StratificationText { get; set; }
            public int? Count { get; set; }
        }

        static Expression CreatePropertyBinding(PropertyInfo prop, IQueryable query, ParameterExpression queryType)
        {
            if (prop.Name == "Count")
            {
                // from the query type (should be a grouping) need to get the IEnumerable type
                Type enumerableType = GetGenericType(typeof(IEnumerable<>), query.ElementType);
                // from the IEnumerable type get the contained generic type (ie enumerable of what)
                Type elementType = enumerableType.GetGenericArguments().First();

                Expression countExpr = Expression.Call(typeof(Enumerable), "Count", new Type[] { elementType }, new Expression[] { queryType });

                return countExpr;
            } 
                    
            //next property will be binding against the key value
            Expression keyProp = Expression.Property(queryType, query.ElementType.GetProperty("Key"));
            //next get the sex property
            Expression sexProp = Expression.Property(keyProp, keyProp.Type.GetProperty("Sex"));

            return sexProp;
        }

        static Type GetGenericType(Type generic, Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == generic) return type;
                if (generic.IsInterface)
                {
                    foreach (Type intfType in type.GetInterfaces())
                    {
                        Type found = GetGenericType(generic, intfType);
                        
                        if (found != null) 
                            return found;
                    }
                }
                type = type.BaseType;
            }
            return null;
        }

    }
}
