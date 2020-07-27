using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.RegexValidation;
using NUnit.Framework;

namespace MonitoringNugetTest
{
    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void IsCustomerNrValid_IsValid_ReturnTrue()
        {
            // Arrange
            var custNr = "CU12345";
            // Act
            var bOK = RegexValidation.CustomerNrValidation(custNr);
            // Assert
            Assert.IsTrue(bOK);
        }

        [Test]
        public void IsCustomerNrValid_IsInValidTooLong_ReturnFalse()
        {
            // Arrange
            var custNr = "CU1234577";
            // Act
            var bOK = RegexValidation.CustomerNrValidation(custNr);
            // Assert
            Assert.IsFalse(bOK);
        }

        [Test]
        public void IsCustomerNrValid_IsInValidTooShort_ReturnFalse()
        {
            // Arrange
            var custNr = "CU123";
            // Act
            var bOK = RegexValidation.CustomerNrValidation(custNr);
            // Assert
            Assert.IsFalse(bOK);
        }

        [Test]
        public void IsCustomerNrValid_IsInValidNoCU_ReturnFalse()
        {
            // Arrange
            var custNr = "12345";
            // Act
            var bOK = RegexValidation.CustomerNrValidation(custNr);
            // Assert
            Assert.IsFalse(bOK);
        }

        [Test]
        public void IsPasswordNrValid_IsValid_ReturnTrue()
        {
            // Arrange
            var pw = "A48569k4";
            // Act
            var bOK = RegexValidation.PasswordValidation(pw);
            // Assert
            Assert.IsTrue(bOK);
        }

        [Test]
        public void IsPasswordNrValid_IsInValidTooShort_ReturnFalse()
        {
            // Arrange
            var pw = "Cu12345";
            // Act
            var bOK = RegexValidation.CustomerNrValidation(pw);
            // Assert
            Assert.IsFalse(bOK);
        }

        [Test]
        public void IsPasswordValid_IsInValidNoUpper_ReturnFalse()
        {
            // Arrange
            var custNr = "cu444412345";
            // Act
            var bOK = RegexValidation.CustomerNrValidation(custNr);
            // Assert
            Assert.IsFalse(bOK);
        }

        [Test]
        public void IsHomepageValid_IsValideHP_ReturnTrue()
        {
            // Arrange
            var hp = @"http://police.stackoverflow.com";
            // Act
            bool ok = RegexValidation.HomepageValidation(hp);
            // Assert
            Assert.IsTrue(ok);
        }

        [Test]
        public void IsHomepageValid_IsValideHPtwo_ReturnTrue()
        {
            // Arrange
            var hp = @"https://www.stackoverflow.com";
            // Act
            bool ok = RegexValidation.HomepageValidation(hp);
            // Assert
            Assert.IsTrue(ok);
        }

        [Test]
        public void IsHomepageValid_IsValideHPthree_ReturnTrue()
        {
            // Arrange
            var hp = @"stackoverflow.com";
            // Act
            bool ok = RegexValidation.HomepageValidation(hp);
            // Assert
            Assert.IsTrue(ok);
        }

        [Test]
        public void IsHomepageValid_IsValideHPfour_ReturnTrue()
        {
            // Arrange
            var hp = @"police.stackoverflow.com";
            // Act
            bool ok = RegexValidation.HomepageValidation(hp);
            // Assert
            Assert.IsTrue(ok);
        }

        [Test]
        public void IsHomepageValid_IsInValideHP_ReturnFalse()
        {
            // Arrange
            var hp = @"http://police.stack.overflow.com";
            // Act
            bool ok = RegexValidation.HomepageValidation(hp);
            // Assert
            Assert.IsFalse(ok);
        }

        [Test]
        public void IsEmailValid_IsValide_ReturnTrue()
        {
            // Arrange
            var email = @"max.mustermann@bluewin.ch";
            // Act
            bool ok = RegexValidation.EMailValidation(email);
            // Assert
            Assert.IsTrue(ok);
        }

        [Test]
        public void IsEmailValid_IsInValide_ReturnFalse()
        {
            // Arrange
            var email = @"max.mustermann@bluewinch";
            // Act
            bool ok = RegexValidation.EMailValidation(email);
            // Assert
            Assert.IsFalse(ok);
        }
    }
}
