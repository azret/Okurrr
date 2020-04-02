﻿using System;
using System.Ai;
using System.Ai.Trainers;
using System.Drawing;
using System.IO;

unsafe partial class Exec {
    public static bool Search(
        App app,
        string cliScript,
        Func<bool> IsTerminated) {
        app.Session.Model.RunFullCosineSearch(cliScript, 17);
        return false;
    }

    public static bool ff103(
        App app,
        string dir,
        Func<bool> IsTerminated) {

        const int CAPACITY = 1048576,
            GENS = (int)1e6,
                 DIMS = 11;

        string outputFileName = Path.ChangeExtension(Path.Combine(dir.TrimEnd('\\'), "ff103.md"), ".md");

        if (app.Session == null) {
            var ff103 = new ff103(new System.Ai.Model(CAPACITY, DIMS));
            ff103.Build();
            app.Session = ff103;
            Model.Dump(ff103.Model.Sort(), ff103.Model.Dims, outputFileName);
        }

        App.StartWin32UI(null,
                       Curves.DrawCurves, () => app.Session, $"{outputFileName} - (Bag of Words w/ Negative Sampling)",
                       Color.White,
                       Properties.Resources.Oxygen,
                       new Size(623, 400));

        Trainer.Run(app.Session,
            GENS,
            IsTerminated);

        Model.Dump(app.Session.Model, app.Session.Model.Dims, outputFileName);

        return false;
    }

    public static bool Iris(
        App app,
        string dir,
        Func<bool> IsTerminated) {

        const int GENS = (int)1e6;

        string outputFileName = Path.ChangeExtension(Path.Combine(dir.TrimEnd('\\'), "Iris.md"), ".md");

        if (app.Session == null) {
            var ff103 = new Iris();
            ff103.Build();
            app.Session = ff103;
            Model.Dump(ff103.Model.Sort(), ff103.Model.Dims, outputFileName);
        }

        App.StartWin32UI(null,
                       Curves.DrawCurves, () => app.Session, $"{outputFileName} - (Iris)",
                       Color.White,
                       Properties.Resources.Oxygen,
                       new Size(623, 400));

        Trainer.Run(app.Session,
            GENS,
            IsTerminated);

        Model.Dump(app.Session.Model, app.Session.Model.Dims, outputFileName);

        return false;
    }
}