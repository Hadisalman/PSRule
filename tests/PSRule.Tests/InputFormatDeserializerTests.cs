﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using PSRule.Pipeline;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Xunit;

namespace PSRule
{
    public sealed class InputFormatDeserializerTests
    {
        [Fact]
        public void DeserializeObjectsYaml()
        {
            var actual = PipelineReceiverActions.ConvertFromYaml(GetYamlContent(), PipelineReceiverActions.PassThru).ToArray();

            Assert.Equal(2, actual.Length);
            Assert.Equal("TestObject1", actual[0].PropertyValue<string>("targetName"));
            Assert.Equal("Test", actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<string>("kind"));
            Assert.Equal(2, actual[1].PropertyValue("spec").PropertyValue("properties").PropertyValue<int>("value2"));
            Assert.Equal(2, actual[1].PropertyValue("spec").PropertyValue("properties").PropertyValue<PSObject[]>("array").Length);
            Assert.Equal("TestObject1", PipelineHookActions.BindTargetName(null, false, actual[0]));
        }

        [Fact]
        public void DeserializeObjectsJson()
        {
            var actual = PipelineReceiverActions.ConvertFromJson(GetJsonContent(), PipelineReceiverActions.PassThru).ToArray();

            Assert.Equal(2, actual.Length);
            Assert.Equal("TestObject1", actual[0].PropertyValue<string>("targetName"));
            Assert.Equal("Test", actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<string>("kind"));
            Assert.Equal(2, actual[1].PropertyValue("spec").PropertyValue("properties").PropertyValue<int>("value2"));
            Assert.Equal(2, actual[1].PropertyValue("spec").PropertyValue("properties").PropertyValue<PSObject[]>("array").Length);
            Assert.Equal("TestObject1", PipelineHookActions.BindTargetName(null, false, actual[0]));

            // Single item
            actual = PipelineReceiverActions.ConvertFromJson(GetJsonContent("Single"), PipelineReceiverActions.PassThru).ToArray();
            Assert.Single(actual);
            Assert.Equal("TestObject1", actual[0].PropertyValue<string>("targetName"));
            Assert.Equal("Test", actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<string>("kind"));

            // Malformed item
            Assert.Throws<PipelineSerializationException>(() => PipelineReceiverActions.ConvertFromJson("{", PipelineReceiverActions.PassThru).ToArray());
            Assert.Throws<PipelineSerializationException>(() => PipelineReceiverActions.ConvertFromJson("{ \"key\": ", PipelineReceiverActions.PassThru).ToArray());
        }

        [Fact]
        public void DeserializeObjectsMarkdown()
        {
            var actual = PipelineReceiverActions.ConvertFromMarkdown(GetMarkdownContent(), PipelineReceiverActions.PassThru).ToArray();

            Assert.Single(actual);
            Assert.Equal("TestObject1", actual[0].PropertyValue<string>("targetName"));
            Assert.Equal("Test", actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<string>("kind"));
            Assert.Equal(1, actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<int>("value1"));
            Assert.Equal(2, actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<PSObject[]>("array").Length);
            Assert.Equal("TestObject1", PipelineHookActions.BindTargetName(null, false, actual[0]));
        }

        [Fact]
        public void DeserializeObjectsPowerShellData()
        {
            var actual = PipelineReceiverActions.ConvertFromPowerShellData(GetDataContent(), PipelineReceiverActions.PassThru).ToArray();

            Assert.Single(actual);
            Assert.Equal("TestObject1", actual[0].PropertyValue<string>("targetName"));
            Assert.Equal("Test", actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<string>("kind"));
            Assert.Equal(1, actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<int>("value1"));
            Assert.Equal(2, actual[0].PropertyValue("spec").PropertyValue("properties").PropertyValue<Array>("array").Length);
            Assert.Equal("TestObject1", PipelineHookActions.BindTargetName(null, false, actual[0]));
        }

        private static string GetYamlContent()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ObjectFromFile.yaml");
            return File.ReadAllText(path);
        }

        private static string GetJsonContent(string suffix = "")
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ObjectFromFile{suffix}.json");
            return File.ReadAllText(path);
        }

        private static string GetMarkdownContent()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ObjectFromFile.md");
            return File.ReadAllText(path);
        }

        private static string GetDataContent()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ObjectFromFile.psd1");
            return File.ReadAllText(path);
        }
    }
}
