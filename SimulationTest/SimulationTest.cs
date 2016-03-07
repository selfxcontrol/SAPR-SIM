using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using Entities.impl;
using System.Collections.Generic;
using Simulation;

namespace SimulationTest
{
    //[TestClass]
    //public class SimulationTest
    //{

    //    Model instance = Model.Instance;


    //    //[TestMethod]
    //    public void test_SimulateSimpleScheme()
    //    {
    //        EntityStart start = new EntityStart();
    //        Project prj = new Project();

    //        EntityDestination finish = new EntityDestination();
    //        Procedure procedure = new Procedure();

    //        Resource res = new Resource();
    //        res.efficiency = 0.8;

    //        procedure.addResource(res);
    //        procedure.setInputs(new List<Entity>() { start });
    //        procedure.setOutputs(new List<Entity>() { finish });
    //        procedure.manHour = 1;

    //        start.setOutputs(new List<Entity>() { procedure });
    //        finish.setInputs(new List<Entity>() { procedure });

    //        instance.addProject(prj);
    //        instance.addEntity(start);
    //        instance.addEntity(procedure);
    //        instance.addEntity(finish);


    //        Simulation.Simulation.simulate();

    //        Assert.IsTrue(prj.state == State.DONE);
    //    }

    //    [TestMethod]
    //    public void test_SimulateSimpleSchemeWithSubModel()
    //    {
    //        EntityStart start = new EntityStart();
    //        Project prj = new Project();
    //        prj.complexity = 1;
    //        Project prj2 = new Project();
    //        prj2.complexity = 1;

    //        EntityDestination finish = new EntityDestination();
    //        Procedure procedure = new Procedure();

    //        EntityStart sub_start = new EntityStart();

    //        EntityDestination sub_finish = new EntityDestination();

    //        Submodel subModel = new Submodel();

    //        Resource res = new Resource();
    //        res.efficiency = 0.8;
    //        res.count = 1;

    //        procedure.addResource(res);
    //        procedure.setInputs(new List<Entity>() { sub_start });
    //        procedure.setOutputs(new List<Entity>() { sub_finish });
    //        procedure.manHour = 1;

    //        sub_start.setOutputs(new List<Entity>() { procedure });
    //        sub_finish.setInputs(new List<Entity>() { procedure });

    //        subModel.addEntity(sub_start);
    //        subModel.addEntity(procedure);
    //        subModel.addEntity(sub_finish);

    //        subModel.setInputs(new List<Entity>() { start });
    //        subModel.setOutputs(new List<Entity>() { finish });

    //        start.setOutputs(new List<Entity>() { subModel });
    //        finish.setInputs(new List<Entity>() { subModel });

    //        instance.addProject(prj);
    //        instance.addProject(prj2);
    //        instance.addEntity(start);
    //        instance.addEntity(subModel);
    //        instance.addEntity(finish);

    //        instance.timeRestriction = 10;


    //        Simulation.Simulation.simulate();

    //        Assert.IsTrue(prj.state == State.DONE);
    //        Assert.IsTrue(prj2.state == State.DONE);

    //    }
    //}
}
