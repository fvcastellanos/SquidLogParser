using System.Collections.Generic;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using SquidLogParser.Data;
using SquidLogParser.Services;
using FluentAssertions;
using LanguageExt.UnitTesting;
using System;
using LanguageExt;

namespace SquidLogParser.Tests.Services
{
    [TestFixture]
    public class FilterUrlServiceTest: ServiceTestBase
    {
        const string TestSite = "http://test-site.com";
        private FilterUrlService _filterUrlService;

        [SetUp]
        public void SetUp()
        {
            _filterUrlService = new FilterUrlService(CreateLogger<FilterUrlService>(), DbContextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            DbContextMock.Reset();
        }

        [Test]
        public void TestAddUrl()
        {
            DbContextMock.Setup(context => context.FilteredUrls)
                .ReturnsDbSet(new List<FilteredUrl>());

            DbContextMock.Setup(context => context.FilteredUrls.Add(It.IsAny<FilteredUrl>()));
            DbContextMock.Setup(context => context.SaveChanges());

            var result = _filterUrlService.AddUrl(TestSite);

            result.ShouldBeRight();

            DbContextMock.Verify(context => context.FilteredUrls);
            DbContextMock.Verify(context => context.FilteredUrls.Add(It.IsAny<FilteredUrl>()));
            DbContextMock.Verify(context => context.SaveChanges());
        }

        [Test]
        public void TestAddExistingUrl() 
        {            
            DbContextMock.Setup(context => context.FilteredUrls)
                .ReturnsDbSet(BuildFilteredUrl(TestSite));

            var result = _filterUrlService.AddUrl(TestSite);

            result.ShouldBeLeft(left => left.Should()
                .Be(string.Format("URL: {0} already exists in filtered url set", TestSite))
            );                

            DbContextMock.Verify(context => context.FilteredUrls);
        }

        [Test]
        public void TestAddUrlThrowsException()
        {
            DbContextMock.Setup(context => context.FilteredUrls)
                .Throws(new Exception("expected exception"));

            var result = _filterUrlService.AddUrl(TestSite);

            result.ShouldBeLeft(left => left.Should()
                .Be(string.Format("Can't add URL: {0} to filtered sites", TestSite))
            );

            DbContextMock.Verify(context => context.FilteredUrls);
        }

        [Test]
        public void TestRemoveUrl()
        {
            DbContextMock.Setup(context => context.FilteredUrls)
                .ReturnsDbSet(BuildFilteredUrl(TestSite));

            var result = _filterUrlService.RemoveUrl(TestSite);

            result.ShouldBeRight();

            DbContextMock.Verify(context => context.FilteredUrls);
            DbContextMock.Verify(context => context.FilteredUrls.Remove(It.IsAny<FilteredUrl>()));
            DbContextMock.Verify(context => context.SaveChanges());
        }

        [Test]
        public void TestRemoveUrlThrowsException()
        {
            DbContextMock.Setup(context => context.FilteredUrls)
                .Throws(new Exception("test exception"));

            var result = _filterUrlService.RemoveUrl(TestSite);

            result.ShouldBeLeft(left => left.Should()
                .Be(string.Format("Can't delete URL: {0}", TestSite))
            );

            DbContextMock.Verify(context => context.FilteredUrls);
        }

        [Test]
        public void TestGetAllFilteredUrls()
        {
            DbContextMock.Setup(context => context.FilteredUrls)
                .ReturnsDbSet(BuildFilteredUrl(TestSite));

            var result = _filterUrlService.GetUrls("");

            result.ShouldBeRight(right => right.Should()
                .BeEquivalentTo(BuildFilteredUrl(TestSite))
            );

            DbContextMock.Verify(context => context.FilteredUrls);
        }

        [Test]
        public void TestGetFilteredUrls()
        {
            DbContextMock.Setup(context => context.FilteredUrls)
                .ReturnsDbSet(BuildFilteredUrl(TestSite));

            var result = _filterUrlService.GetUrls("test-site");

            result.ShouldBeRight(right => right.Should()
                .BeEquivalentTo(BuildFilteredUrl(TestSite))
            );

            DbContextMock.Verify(context => context.FilteredUrls);
        }

        [Test]
        public void TestGetFilteredUrlsThrowsException()
        {
            DbContextMock.Setup(context => context.FilteredUrls)
                .Throws(new Exception("test exception"));

            var result = _filterUrlService.GetUrls(TestSite);

            result.ShouldBeLeft(left => left.Should()
                .Be("Can't get URL list")
            );

            DbContextMock.Verify(context => context.FilteredUrls);
        }

        // ----------------------------------------------------------------------------------------------------

        private List<FilteredUrl> BuildFilteredUrl(string url)
        {
            return new List<FilteredUrl>()
            {
                new FilteredUrl()
                {
                    Url = url
                }
            };
        }
    }
}