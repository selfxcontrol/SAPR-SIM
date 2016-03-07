using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using Entities.impl;
using EntityValidator;
using EntityValidator.exeptions;
using EntityValidator.validator;

namespace EntityValidatorTest.systemValidatorTest
{
    [TestClass]
    public class SystemValidatorTest_shouldThrowNotOneDestException
    {
        Model instance = Model.Instance;

        [TestCleanup]
        public void clean()
        {
            instance.getEntities().Clear();
        }

        [TestMethod]
        public void test_shouldThrowNotOneStartExeption()
        {
            instance.addEntity(new EntityStart());
            instance.addEntity(new EntityStart());
            instance.addEntity(new EntityDestination());

            IValidator validator = new SystemValidator(instance.getEntities(), instance.getResources());

            Assert.IsFalse(validator.startValidation().Success);
        }
    }
}
