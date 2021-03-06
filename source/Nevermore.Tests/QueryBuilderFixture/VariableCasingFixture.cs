﻿using System.Linq;
using FluentAssertions;
using Nevermore.Contracts;
using NSubstitute;
using Xunit;

namespace Nevermore.Tests.QueryBuilderFixture
{
    public class VariableCasingFixture
    {
        private IRelationalTransaction transaction;
        private string query = null;
        private CommandParameters parameters = null;

        public VariableCasingFixture()
        {
            query = null;
            parameters = null;
            transaction = Substitute.For<IRelationalTransaction>();
            transaction.WhenForAnyArgs(c => c.ExecuteReader<IId>("", Arg.Any<CommandParameters>()))
                .Do(c =>
                {
                    query = c.Arg<string>();
                    parameters = c.Arg<CommandParameters>();
                });
        }

        [Fact]
        public void VariablesCasingIsNormalisedForWhere()
        {
            new QueryBuilder<IId>(transaction, "Order")
                .Where("fOo = @myVAriabLe AND Baz = @OthervaR")
                .Parameter("MyVariable", "Bar")
                .Parameter("OTHERVAR", "Bar")
                .ToList();

            parameters.Count.Should().Be(2);
            foreach (var parameter in parameters)
                query.Should().Contain("@" + parameter.Key, "Should contain @" + parameter.Key);
        }

        [Fact]
        public void VariablesCasingIsNormalisedForWhereSingleParam()
        {
            new QueryBuilder<IId>(transaction, "Order")
                .Where("fOo", SqlOperand.GreaterThan, "Bar")
                .ToList();

            parameters.Count.Should().Be(1);
            var parameter = "@" + parameters.Keys.Single();
            query.Should().Contain(parameter, "Should contain " + parameter);
        }

        [Fact]
        public void VariablesCasingIsNormalisedForWhereTwoParam()
        {
            new QueryBuilder<IId>(transaction, "Order")
                .Where("fOo", SqlOperand.Between, 1, 2)
                .ToList();

            parameters.Count.Should().Be(2);
            foreach (var parameter in parameters)
                query.Should().Contain("@" + parameter.Key, "Should contain @" + parameter.Key);
        }

        [Fact]
        public void VariablesCasingIsNormalisedForWhereParamArray()
        {
            new QueryBuilder<IId>(transaction, "Order")
                .Where("fOo", SqlOperand.Contains, new[] { 1, 2, 3 })
                .ToList();

            parameters.Count.Should().Be(1);
            var parameter = "@" + parameters.Keys.Single();
            query.Should().Contain(parameter, "Should contain " + parameter);
        }

        [Fact]
        public void VariablesCasingIsNormalisedForWhereIn()
        {
            new QueryBuilder<IId>(transaction, "Order")
                .Where("fOo", SqlOperand.In, new[] { "BaR", "BaZ" })
                .ToList();

            parameters.Count.Should().Be(2);
            foreach (var parameter in parameters)
                query.Should().Contain("@" + parameter.Key, "Should contain @" + parameter.Key);
        }
    }
}