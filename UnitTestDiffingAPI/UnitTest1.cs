using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DiffingAPI.Models;
using DiffingAPI.Controllers;
using System.Web.Http.Results;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace UnitTestDiffingAPI
{
    [TestClass]
    public class UnitTest1
    {
        private int vId = 2;

        [TestMethod]
        public void TestMethod1() //Test GET /v1/diff/1 with left and right have not data
        {
            var controller = new DiffController();
            var result = controller.Get(vId);
            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public void TestMethod2() //Test PUT /v1/diff/1/left data: "AAAAAA=="
        {
            var controller = new DiffController();
            var result = controller.Create(vId, "left", new DiffData { data = "AAAAAA==" }) as ResponseMessageResult;
            Assert.AreEqual(result.Response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(result.Response.Content, null);
        }

        [TestMethod]
        public void TestMethod3() //Test GET /v1/diff/1 with left have data and right have not
        {
            var controller = new DiffController();
            var result = controller.Get(vId);
            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public void TestMethod4() //Test PUT /v1/diff/1/right data: "AAAAAA=="
        {
            var controller = new DiffController();
            var result = controller.Create(vId, "right", new DiffData { data = "AAAAAA==" }) as ResponseMessageResult;
            Assert.AreEqual(result.Response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(result.Response.Content, null);
        }

        [TestMethod]
        public void TestMethod5() //Test GET /v1/diff/1 with left = right
        {
            var controller = new DiffController();
            var result = controller.Get(vId) as OkNegotiatedContentResult<DiffResult>;
           
            Assert.AreEqual(JsonConvert.SerializeObject(result.Content), JsonConvert.SerializeObject(new DiffResult { diffResultType = "Equals" }));
        }

        [TestMethod]
        public void TestMethod6() //Test PUT /v1/diff/1/right data: "AQABAQ=="
        {
            var controller = new DiffController();
            var result = controller.Create(vId, "right", new DiffData { data = "AQABAQ==" }) as ResponseMessageResult;
            Assert.AreEqual(result.Response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(result.Response.Content, null);
        }

        [TestMethod]
        public void TestMethod7() //Test GET /v1/diff/1 with left and right have data
        {
            var controller = new DiffController();
            var result = controller.Get(vId) as OkNegotiatedContentResult<DiffResultWithDiffs>;

            DiffResultWithDiffs diffResult = new DiffResultWithDiffs();
            diffResult.diffResultType = "ContentDoNotMatch";
            List<DiffPart> diffs = new List<DiffPart>();
            DiffPart diffPart1 = new DiffPart();
            diffPart1.Length = 1;
            diffPart1.Offset = 0;
            diffs.Add(diffPart1);
            DiffPart diffPart2 = new DiffPart();
            diffPart2.Length = 2;
            diffPart2.Offset = 2;
            diffs.Add(diffPart2);
            diffResult.diffs = diffs;
            Assert.AreEqual(JsonConvert.SerializeObject(result.Content), JsonConvert.SerializeObject(diffResult));
        }

        [TestMethod]
        public void TestMethod8() //Test PUT /v1/diff/1/left data: "AAA="
        {
            var controller = new DiffController();
            var result = controller.Create(vId, "left", new DiffData { data = "AAA=" }) as ResponseMessageResult;
            Assert.AreEqual(result.Response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(result.Response.Content, null);
        }

        [TestMethod]
        public void TestMethod9() //Test GET /v1/diff/1 with left and right have data
        {
            var controller = new DiffController();
            var result = controller.Get(vId) as OkNegotiatedContentResult<DiffResult>;

            DiffResult diffResult = new DiffResult();
            diffResult.diffResultType = "SizeDoNotMatch";
            Assert.AreEqual(JsonConvert.SerializeObject(result.Content), JsonConvert.SerializeObject(diffResult));
        }

        [TestMethod]
        public void TestMethod10() //Test PUT /v1/diff/1/left data: null
        {
            var controller = new DiffController();
            var result = controller.Create(vId, "left", new DiffData { data = null });
            Assert.IsTrue(result is BadRequestResult);
        }
    }


}
