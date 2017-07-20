using System;
using System.Collections.Generic;
//using Lpp.Data.Composition;
using System.Data.Entity;
using System.ComponentModel.Composition;

namespace Lpp.Dns.HealthCare.Summary
{
    // TODO Obsolete?
    //[Export( typeof( IPersistenceDefinition<SummaryDomain> ) )]
    //public class SummaryPersistence : IPersistenceDefinition<SummaryDomain>
    //{
    //    public void BuildModel( DbModelBuilder builder )
    //    {
    //        builder.Entity<StratificationCategoryLookUp>()
    //            .HasKey(s => new { s.StratificationCategoryId, s.StratificationType })
    //            .ToTable("StratificationCategoryLookUp");
    //        builder.Entity<StratificationAgeRangeMapping>()
    //            .HasKey( s => new { s.AgeStratificationCategoryId, s.AgeClassification } )
    //            .ToTable( "StratificationAgeRangeMapping" );
    //        // No longer using this table. Using distinct union of DataMartAvailabilityPeriods instead.
    //        //builder.Entity<DataAvailabilityPeriodLookUp>()
    //        //    .HasKey( s => new { s.Period } )
    //        //    .ToTable( "DataAvailabilityPeriod" );
    //        builder.Entity<DataAvailabilityPeriodCategoryLookUp>()
    //            .HasKey( s => new { s.CategoryTypeId } )
    //            .ToTable( "DataAvailabilityPeriodCategory" );
    //        builder.Entity<DataMartAvailabilityPeriodsLookUp>()
    //            .HasKey( s => new { s.QueryId, s.DataMartId, s.QueryTypeId, s.Period } )
    //            .ToTable( "DataMartAvailabilityPeriods" );
    //        builder.Entity<LookUpQueryTypeMetricsView>()
    //            .HasKey( s => new { s.QueryTypeId, s.MetricId } )
    //            .ToTable( "LookUpQueryTypeMetrics_view" );
    //    }
    //}
}