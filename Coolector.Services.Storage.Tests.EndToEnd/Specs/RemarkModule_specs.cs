﻿using System;
using System.Collections.Generic;
using System.IO;
using Coolector.Services.Storage.Tests.EndToEnd.Framework;
using System.Linq;
using Coolector.Common.Dto.Remarks;
using Machine.Specifications;

namespace Coolector.Services.Storage.Tests.EndToEnd.Specs
{
    public abstract class RemarkModule_specs
    {
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:10000");
        protected static RemarkDto Remark;
        protected static IEnumerable<RemarkDto> Remarks;
        protected static IEnumerable<RemarkCategoryDto> Categories;
        protected static Guid RemarkId;

        protected static void InitializeAndFetch()
        {
            var remark = FetchRemarks().First();
            RemarkId = remark.Id;
        }

        protected static IEnumerable<RemarkDto> FetchRemarks()
            => HttpClient.GetAsync<IEnumerable<RemarkDto>>("remarks?latest=true").WaitForResult();

        protected static IEnumerable<RemarkDto> FetchNearestRemarks()
            => HttpClient.GetCollectionAsync<RemarkDto>("remarks?results=100&radius=10000&longitude=1.0&latitude=1.0").WaitForResult();

        protected static IEnumerable<RemarkDto> GetRemarksWithCategory(string categoryName)
            => HttpClient.GetCollectionAsync<RemarkDto>($"remarks?radius=10000&longitude=1.0&latitude=1.0&categories={categoryName}").WaitForResult();

        protected static IEnumerable<RemarkDto> GetRemarksWithState(string state)
            => HttpClient.GetCollectionAsync<RemarkDto>($"remarks?radius=10000&longitude=1.0&latitude=1.0&state={state}").WaitForResult();

        protected static IEnumerable<RemarkCategoryDto> FetchCategories()
            => HttpClient.GetAsync<IEnumerable<RemarkCategoryDto>>("remarks/categories").WaitForResult();

        protected static RemarkDto FetchRemark(Guid id)
            => HttpClient.GetAsync<RemarkDto>($"remarks/{id}").WaitForResult();
    }

    [Subject("StorageService fetch remarks")]
    public class when_fetching_remarks : RemarkModule_specs
    {
        Because of = () => Remarks = FetchRemarks();

        It should_not_be_null = () => Remarks.ShouldNotBeNull();

        It should_not_be_empty = () => Remarks.ShouldNotBeEmpty();
    }

    [Subject("StorageService fetch nearest remarks")]
    public class when_fetching_nearest_remarks : RemarkModule_specs
    {
        Because of = () => Remarks = FetchNearestRemarks();

        It should_not_be_null = () => Remarks.ShouldNotBeNull();

        It should_not_be_empty = () => Remarks.ShouldNotBeEmpty();

        It should_return_remarks_in_correct_order = () =>
        {
            RemarkDto previousRemark = null;
            foreach (var remark in Remarks)
            {
                if (previousRemark != null)
                {
                    previousRemark.Location.Coordinates[0].ShouldBeLessThanOrEqualTo(remark.Location.Coordinates[0]);
                    previousRemark.Location.Coordinates[1].ShouldBeLessThanOrEqualTo(remark.Location.Coordinates[1]);
                }
                previousRemark = remark;
            }
        };
    }

    [Subject("StorageService fetch remarks with category")]
    public class when_fetching_remarks_with_specific_category : RemarkModule_specs
    {
        protected static string Category = "damages";

        Because of = () => Remarks = GetRemarksWithCategory(Category);

        It should_return_non_empty_collection = () =>
        {
            Remarks.ShouldNotBeEmpty();
            foreach (var remark in Remarks)
            {
                remark.Id.ShouldNotEqual(Guid.Empty);
                remark.Author.UserId.ShouldNotBeEmpty();
                remark.Author.Name.ShouldNotBeEmpty();
                remark.Category.Id.ShouldNotEqual(Guid.Empty);
                remark.Category.Name.ShouldNotBeEmpty();
                remark.Location.Coordinates.Length.ShouldEqual(2);
                remark.Location.Coordinates[0].ShouldNotEqual(0);
                remark.Location.Coordinates[1].ShouldNotEqual(0);
            }
        };

        It should_contain_remarks_with_the_same_category = () 
            => Remarks.All(x => x.Category.Name == Category).ShouldBeTrue();
    }

    [Subject("StorageService fetch resolved remarks")]
    public class when_fetching_resolved_remarks : RemarkModule_specs
    {
        protected static string State = "resolved";
        
        Because of = () => Remarks = GetRemarksWithState(State);

        It should_return_non_empty_collection = () =>
        {
            Remarks.ShouldNotBeEmpty();
            foreach (var remark in Remarks)
            {
                remark.Id.ShouldNotEqual(Guid.Empty);
                remark.Author.UserId.ShouldNotBeEmpty();
                remark.Author.Name.ShouldNotBeEmpty();
                remark.Category.Id.ShouldNotEqual(Guid.Empty);
                remark.Category.Name.ShouldNotBeEmpty();
                remark.Location.Coordinates.Length.ShouldEqual(2);
                remark.Location.Coordinates[0].ShouldNotEqual(0);
                remark.Location.Coordinates[1].ShouldNotEqual(0);
            }
        };

        It should_contain_only_resolved_remarks = () 
            => Remarks.All(x => x.Resolved).ShouldBeTrue();
    }

    [Subject("StorageService fetch active remarks")]
    public class when_fetching_active_remarks : RemarkModule_specs
    {
        protected static string State = "active";

        Because of = () => Remarks = GetRemarksWithState(State);

        It should_return_non_empty_collection = () =>
        {
            Remarks.ShouldNotBeEmpty();
            foreach (var remark in Remarks)
            {
                remark.Id.ShouldNotEqual(Guid.Empty);
                remark.Author.UserId.ShouldNotBeEmpty();
                remark.Author.Name.ShouldNotBeEmpty();
                remark.Category.Id.ShouldNotEqual(Guid.Empty);
                remark.Category.Name.ShouldNotBeEmpty();
                remark.Location.Coordinates.Length.ShouldEqual(2);
                remark.Location.Coordinates[0].ShouldNotEqual(0);
                remark.Location.Coordinates[1].ShouldNotEqual(0);
            }
        };

        It should_return_active_remarks = () 
            => Remarks.All(x => x.Resolved == false).ShouldBeTrue();
    }

    [Subject("StorageService fetch single remark")]
    public class when_fetching_single_remark : RemarkModule_specs
    {
        Establish context = () => InitializeAndFetch();

        Because of = () => Remark = FetchRemark(RemarkId);

        It should_not_be_null = () => Remark.ShouldNotBeNull();

        It should_have_correct_id = () => Remark.Id.ShouldEqual(RemarkId);

        It should_return_remark = () =>
        {
            Remark.Id.ShouldNotEqual(Guid.Empty);
            Remark.Author.ShouldNotBeNull();
            Remark.Category.ShouldNotBeNull();
            Remark.CreatedAt.ShouldNotEqual(default(DateTime));
            Remark.Location.ShouldNotBeNull();
        };
    }

    [Subject("StorageService fetch categories")]
    public class when_fetching_categories : RemarkModule_specs
    {
        Because of = () => Categories = FetchCategories();

        It should_not_be_null = () => Categories.ShouldNotBeNull();

        It should_not_be_empty = () => Categories.ShouldNotBeEmpty();
    }
}