/********************************************************************************************
 * Copyright (c) Computer Graphics Course by Fayoum University 
 * Prof. Amr M. Gody, amg00@fayoum.edu.eg
 * License: free for use and distribution for Educational purposes. It is required to keep this header comments on your code. 
 * Purpose:            main window logic and user interface logic are here
 *
 * Ver  Date         By     Purpose
 * ---  ----------- -----   --------------------------------------------------------------------
 * 01   2020-12-05  AMG     Created the initial version.
 *************************************************************************************************/

using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System.Windows;

using ComputerGraphics.GraphObjects;

namespace ComputerGraphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BTN_start_Click(object sender, RoutedEventArgs e)
        {
            GameWindowSettings windowSettings = new GameWindowSettings()
            {
                IsMultiThreaded = false,
                RenderFrequency = 24,
                UpdateFrequency = 24
            };
            NativeWindowSettings nws = new NativeWindowSettings()
            {
                Title = "Solar System", Size = new Vector2i(1280, 720)
                
            };
            
           using( OpenGLWindow ogl = new OpenGLWindow(GameWindowSettings.Default ,nws))
            {
                ogl.AddGraph(new Sun(109.0f));
                ogl.AddGraph(new Planet(Planet.Planets.Mercury, 40f, new Vector3(0,0,-300), "resources\\Earth_nightmap.jpg")
                {
            
             
                    _material = new Vector4(2f, 7f, .7f, 2.0f),
                    _rotaionSpeed = 1f,
                    _orbitSpeed = -1f
            
                });
                ogl.Run();
            }
        }

 
    }
}
