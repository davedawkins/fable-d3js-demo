module App

open Fable.Core
open Fable.Core.JsInterop

let d3 = D3.d3

// This code is ported from:
// https://code.likeagirl.io/how-to-set-up-d3-js-with-webpack-and-babel-7bd3f5e20df7
// It's used just to make sure basic d3.js is working

d3.selectAll("rect")
    .transition()
    .delay(400.)
    .duration(400.)
    .style("fill", "orange")
    |> ignore

[<Emit("function () { $0(this); }")>]
let inline thisFunc (f: (obj->unit)) = jsNative

// This code is ported from:
// http://christopheviau.com/d3_tutorial/
// Sections:
// - Simple example
// - Animation chaining

// I have no idea what the real generic types should be
// Perhaps we can improve the binding by removing them from the Type definition
// and only place them on the member signature
// NOTE: I am not sure if that's possible or no
let sampleSVG : D3.Selection.Selection<obj,obj,Browser.Types.HTMLElement,obj option> =
    d3.select("#hover-demo")
        .append("svg")
        .attr("width", 100)
        .attr("height", 100)

sampleSVG.append("circle")
        .style("stroke", "gray")
        .style("fill", "white")
        .attr("r", 40)
        .attr("cx", 50)
        .attr("cy", 50)
        .on("mouseover", thisFunc <| fun self ->
            d3.select(self) // Hack to mack the compiler happy, preferrable to find another way to do it
                .style("fill", "aliceblue")
            |> ignore
        )
        .on("mouseout", thisFunc <| fun self ->
            d3.select(self) // Hack to mack the compiler happy, preferrable to find another way to do it
                .style("fill", "white")
            |> ignore
        )
        |> ignore

// Animation demo

let animationSVG : D3.Selection.Selection<obj,obj,Browser.Types.HTMLElement,obj option> =
    d3.select("#animation-demo")
        .append("svg")
        .attr("width", 100)
        .attr("height", 100)

/// Please note that in order to have `d3.select(unsafeJsThis)` work
/// it's important to `inline` the function
let animateSecondStep (self : obj) =
    d3.select(self)
      .transition()
        .duration(1000.)
        .attr("r", 40)
    |> ignore

/// Please note that in order to have `d3.select(unsafeJsThis)` work
/// it's important to `inline` the function
let animateFirstStep (self : obj) =
    d3.select(self)
        .transition()
        .delay(0.)
        .duration(1000.)
        .attr("r", 10)
        .on("end", fun _ -> animateSecondStep self )
    |> ignore

animationSVG.append("circle")
        .style("stroke", "gray")
        .style("fill", "white")
        .attr("r", 40)
        .attr("cx", 50)
        .attr("cy", 50)
        .on("mousedown", thisFunc animateFirstStep)
        |> ignore
