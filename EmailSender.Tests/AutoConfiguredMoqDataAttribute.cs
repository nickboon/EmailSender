using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace EmailSender.Tests
{
    public class AutoConfiguredMoqDataAttribute : AutoDataAttribute
    {
        public AutoConfiguredMoqDataAttribute()
            : base(() => new Fixture().Customize(new AutoConfiguredMoqCustomization()))
        {
        }
    }
}