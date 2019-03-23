﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace Shazzam.Shaders {
	
	/// <summary>A transition effect </summary>
	public class CircleRevealTransitionEffect : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(CircleRevealTransitionEffect), 0);
		public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(CircleRevealTransitionEffect), new UIPropertyMetadata(((double)(30D)), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty FuzzyAmountProperty = DependencyProperty.Register("FuzzyAmount", typeof(double), typeof(CircleRevealTransitionEffect), new UIPropertyMetadata(((double)(0.01D)), PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty CircleSizeProperty = DependencyProperty.Register("CircleSize", typeof(double), typeof(CircleRevealTransitionEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(2)));
		public static readonly DependencyProperty CenterPointProperty = DependencyProperty.Register("CenterPoint", typeof(Point), typeof(CircleRevealTransitionEffect), new UIPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(3)));
		public static readonly DependencyProperty Texture2Property = ShaderEffect.RegisterPixelShaderSamplerProperty("Texture2", typeof(CircleRevealTransitionEffect), 1);
		public CircleRevealTransitionEffect() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("WpfEyeTrackingLab;component/CircleRevealTransitionEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(ProgressProperty);
			this.UpdateShaderValue(FuzzyAmountProperty);
			this.UpdateShaderValue(CircleSizeProperty);
			this.UpdateShaderValue(CenterPointProperty);
			this.UpdateShaderValue(Texture2Property);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		/// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
		public double Progress {
			get {
				return ((double)(this.GetValue(ProgressProperty)));
			}
			set {
				this.SetValue(ProgressProperty, value);
			}
		}
		/// <summary>The fuzziness factor. </summary>
		public double FuzzyAmount {
			get {
				return ((double)(this.GetValue(FuzzyAmountProperty)));
			}
			set {
				this.SetValue(FuzzyAmountProperty, value);
			}
		}
		/// <summary>The size of the circle. </summary>
		public double CircleSize {
			get {
				return ((double)(this.GetValue(CircleSizeProperty)));
			}
			set {
				this.SetValue(CircleSizeProperty, value);
			}
		}
		/// <summary>The Center point of the effect </summary>
		public Point CenterPoint {
			get {
				return ((Point)(this.GetValue(CenterPointProperty)));
			}
			set {
				this.SetValue(CenterPointProperty, value);
			}
		}
		public Brush Texture2 {
			get {
				return ((Brush)(this.GetValue(Texture2Property)));
			}
			set {
				this.SetValue(Texture2Property, value);
			}
		}
	}
}
