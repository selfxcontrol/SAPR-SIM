using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using Entities.impl;
using EntityValidator;
using EntityValidator.exeptions;
using EntityValidator.validator;

namespace EntityValidatorTest.systemValidatorTest
{
    //[TestClass]
    //public class SystemValidatorTest_EverythingIsFineTest
    //{
    //    Model instance = Model.Instance;

    //    [TestCleanup]
    //    public void clean()
    //    {
    //        instance.getEntities().Clear();
    //    }

    //    [TestMethod]
    //    public void test_EverythingIsFineTest()
    //    {
    //        //creating testModel
    //        EntityStart start = new EntityStart();
    //        Project prj = new Project();

    //        EntityDestination finish = new EntityDestination();
    //        Procedure procedure = new Procedure();

    //        Resource res = new Resource() { count = 1 };
    //        res.efficiency = 0.8;

    //        procedure.addResource(res);
    //        procedure.setInputs(new List<Entity>(){ start });
    //        procedure.setOutputs(new List<Entity>() { finish });
    //        procedure.manHour = 1;

    //        start.setOutputs(new List<Entity>() { procedure });
    //        finish.setInputs(new List<Entity>() { procedure });

    //        instance.addProject(prj);
    //        instance.addResource(res);
    //        instance.addEntity(start);
    //        instance.addEntity(finish);
    //        instance.addEntity(procedure);

    //        IValidator validator = new SystemValidator(instance.getEntities(), instance.getResources());
            
    //        Assert.IsTrue(validator.startValidation().Success);
    //    }   
        
    //}
}
