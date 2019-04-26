﻿namespace Rocket.Chat.Net.Tests.Models
{
    using System;

    using FluentAssertions;

    using Newtonsoft.Json.Linq;

    using NSubstitute;

    using Ploeh.AutoFixture;

    using Rocket.Chat.Net.Collections;
    using Rocket.Chat.Net.Models.Collections;
    using Rocket.Chat.Net.Tests.Models.Fixtures;

    using Xunit;

    [Trait("Category", "Models")]
    public class StreamCollectionFacts
    {
        private static readonly Fixture AutoFixture = new Fixture();

        private readonly string _name = AutoFixture.Create<string>();

        [Fact]
        public void When_object_is_added_and_collection_is_empty_add_new_object()
        {
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };

            var collection = new StreamCollection(_name);

            // Act
            collection.Added(item.Id, JObject.FromObject(item));

            // Assert
            var result = collection.GetAnonymousTypeById(item.Id, item);

            result.Should().Be(item);
        }

        [Fact]
        public void When_object_exists_object_should_exist()
        {
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };

            var collection = new StreamCollection(_name);

            // Act
            collection.Added(item.Id, JObject.FromObject(item));
            var exists = collection.ContainsId(item.Id);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public void When_object_does_not_exists_object_should_not_exist()
        {
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };

            var collection = new StreamCollection(_name);

            // Act
            var exists = collection.ContainsId(item.Id);

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public void When_objects_exists_return_jobject()
        {
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };

            var collection = new StreamCollection(_name);

            // Act
            collection.Added(item.Id, JObject.FromObject(item));

            // Assert
            var result = collection.GetJObjectById(item.Id);

            ((string) result["Id"]).Should().Be(item.Id);
        }

        [Fact]
        public void When_objects_not_exists_return_jobject()
        {
            var collection = new StreamCollection(_name);

            // Act

            // Assert
            var result = collection.GetJObjectById(AutoFixture.Create<string>());

            ((object) result).Should().BeNull();
        }

        [Fact]
        public void When_name_is_set_store_it()
        {
            var collection = new StreamCollection(_name);
            var name = AutoFixture.Create<string>();

            // Act
            collection.Name = name;

            // Assert
            collection.Name.Should().Be(name);
        }

        [Fact]
        public void When_object_is_added_and_collection_is_contains_id_override_new_object()
        {
            var existing = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };

            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };

            var collection = new StreamCollection(_name);

            // Act
            collection.Added(existing.Id, JObject.FromObject(existing));
            collection.Added(existing.Id, JObject.FromObject(item));

            // Assert
            var result = collection.GetAnonymousTypeById(existing.Id, item);

            result.Should().Be(item);
        }

        [Fact]
        public void When_object_is_changed_merge_existing_values()
        {
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    OldValue = AutoFixture.Create<string>(),
                    Value = AutoFixture.Create<string>()
                }
            };

            var other = new
            {
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    OldVaue = item.Obj.OldValue,
                    Value = AutoFixture.Create<string>(),
                    NewValue = AutoFixture.Create<string>()
                },
                NewValue = AutoFixture.Create<string>()
            };

            var collection = new StreamCollection(_name);

            // Act
            collection.Added(item.Id, JObject.FromObject(item));
            collection.Changed(item.Id, JObject.FromObject(other));

            // Assert
            var result = collection.GetAnonymousTypeById(item.Id, other);

            result.Int.Should().Be(other.Int);
            result.Str.Should().Be(other.Str);
            result.Obj.Should().Be(other.Obj);

            other.Should().NotBeSameAs(result);

            var result2 = collection.GetAnonymousTypeById(item.Id, item);
            result2.Id.Should().Be(item.Id);

            item.Should().NotBeSameAs(result2);
        }

        [Fact]
        public void When_an_empty_object_is_given_for_change_update_nothing()
        {
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };

            var other = new
            {
            };

            var collection = new StreamCollection(_name);

            // Act
            collection.Added(item.Id, JObject.FromObject(item));
            collection.Changed(item.Id, JObject.FromObject(other));

            // Assert
            var result = collection.GetAnonymousTypeById(item.Id, item);

            result.Should().Be(item);
        }

        [Fact]
        public void When_object_doesnt_exist_return_null()
        {
            var collection = new StreamCollection(_name);

            // Act
            var result = collection.GetById<StreamCollectionFixture>(AutoFixture.Create<string>());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void When_object_does_exist_return_object()
        {
            var fixture = AutoFixture.Create<StreamCollectionFixture>();
            var collection = new StreamCollection(_name);
            collection.Added(fixture.Id, JObject.FromObject(fixture));

            // Act
            var result = collection.GetById<StreamCollectionFixture>(fixture.Id);

            // Assert
            result.Should().Be(fixture);
        }

        [Fact]
        public void When_object_does_exist_but_generic_is_not_of_type_thow()
        {
            var fixture = AutoFixture.Create<StreamCollectionFixture>();
            var collection = new StreamCollection(_name);
            collection.Added(fixture.Id, JObject.FromObject(fixture));

            // Act
            Action action = () => collection.GetById<string>(fixture.Id);

            // Assert
            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void When_object_deleted_remove_from_collection()
        {
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };
            var collection = new StreamCollection(_name);

            // Act
            collection.Added(item.Id, JObject.FromObject(item));
            collection.Removed(item.Id);

            // Assert
            var result = collection.GetAnonymousTypeById(item.Id, item);

            result.Should().BeNull();
        }

        [Fact]
        public void When_object_added_modified_event_should_be_called()
        {
            var subscriber = Substitute.For<IDummySubscriber>();
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };
            var jObject = JObject.FromObject(item);
            var collection = new StreamCollection(_name);
            collection.Modified += subscriber.React;

            // Act
            collection.Added(item.Id, jObject);

            // Assert
            subscriber.Received()
                      .React(Arg.Any<object>(), Arg.Is<StreamCollectionEventArgs>(args =>
                          args.ModificationType == ModificationType.Added && args.Result == jObject
                          ));
        }

        [Fact]
        public void When_object_changed_modified_event_should_be_called()
        {
            var subscriber = Substitute.For<IDummySubscriber>();
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };
            var jObject = JObject.FromObject(item);
            var collection = new StreamCollection(_name);
            collection.Added(item.Id, jObject);
            collection.Modified += subscriber.React;

            // Act
            collection.Changed(item.Id, jObject);

            // Assert
            subscriber.Received()
                      .React(Arg.Any<object>(), Arg.Is<StreamCollectionEventArgs>(args =>
                          args.ModificationType == ModificationType.Changed && args.Result == jObject
                          ));
        }

        [Fact]
        public void When_object_removed_modified_event_should_be_called()
        {
            var subscriber = Substitute.For<IDummySubscriber>();
            var item = new
            {
                Id = AutoFixture.Create<string>(),
                Str = AutoFixture.Create<string>(),
                Int = AutoFixture.Create<int>(),
                Obj = new
                {
                    Value = AutoFixture.Create<string>()
                }
            };
            var jObject = JObject.FromObject(item);
            var collection = new StreamCollection(_name);
            collection.Added(item.Id, jObject);
            collection.Modified += subscriber.React;

            // Act
            collection.Removed(item.Id);

            // Assert
            subscriber.Received()
                      .React(Arg.Any<object>(), Arg.Is<StreamCollectionEventArgs>(args =>
                          args.ModificationType == ModificationType.Removed && args.Result == jObject
                          ));
        }
    }
}