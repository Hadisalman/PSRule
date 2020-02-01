﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using PSRule.Configuration;
using PSRule.Resources;
using PSRule.Rules;
using System;
using System.Text;

namespace PSRule.Pipeline.Output
{
    internal sealed class CsvOutputWriter : SerializationOutputWriter<InvokeResult>
    {
        private const char COMMA = ',';
        private const char QUOTE = '"';

        private readonly StringBuilder _Builder;

        internal CsvOutputWriter(PipelineWriter inner, PSRuleOption option)
            : base(inner, option)
        {
            _Builder = new StringBuilder();
        }

        public override void WriteObject(object o, bool enumerate)
        {
            if (!(o is InvokeResult result))
                return;

            Add(result);
        }

        protected override string Serialize(InvokeResult[] o)
        {
            WriteHeader();
            foreach (var result in o)
            {
                foreach (var record in result.AsRecord())
                    VisitRecord(record: record);
            }
            return _Builder.ToString();
        }

        private void WriteHeader()
        {
            _Builder.Append(ViewStrings.RuleName);
            _Builder.Append(COMMA);
            _Builder.Append(ViewStrings.TargetName);
            _Builder.Append(COMMA);
            _Builder.Append(ViewStrings.TargetType);
            _Builder.Append(COMMA);
            _Builder.Append(ViewStrings.Outcome);
            _Builder.Append(COMMA);
            _Builder.Append(ViewStrings.OutcomeReason);
            _Builder.Append(COMMA);
            _Builder.Append(ViewStrings.Synopsis);
            _Builder.Append(COMMA);
            _Builder.Append(ViewStrings.Recommendation);
            _Builder.Append(Environment.NewLine);
        }

        private void VisitRecord(RuleRecord record)
        {
            if (record == null)
                return;

            WriteColumn(record.RuleName);
            _Builder.Append(COMMA);
            WriteColumn(record.TargetName);
            _Builder.Append(COMMA);
            WriteColumn(record.TargetType);
            _Builder.Append(COMMA);
            WriteColumn(record.Outcome.ToString());
            _Builder.Append(COMMA);
            WriteColumn(record.OutcomeReason.ToString());
            _Builder.Append(COMMA);
            WriteColumn(record.Info.Synopsis);
            _Builder.Append(COMMA);
            WriteColumn(record.Info.Recommendation);
            _Builder.Append(Environment.NewLine);
        }

        private void WriteColumn(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            _Builder.Append(QUOTE);
            _Builder.Append(value.Replace("\"", "\"\""));
            _Builder.Append(QUOTE);
        }
    }
}