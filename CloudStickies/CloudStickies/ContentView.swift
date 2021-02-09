//
//  ContentView.swift
//  CloudStickies
//
//  Created by Alex Valente on 09/02/2021.
//

import SwiftUI
import CoreData

struct Joke{
    var setup: String
    var punchline: String
    var rating: String
}

struct ContentView: View {
    let jokes = [
        Joke(setup: "What's a cow's favourite place?", punchline: "A mooseum", rating: "Silence")
        
        Joke(setup: "What's brown and sticky?", punchline: "A stick!", rating: "Sigh")
        
        Joke(setup: "What's orange and sounds like a parrot?", punchline: "A carrot!", rating: "Smirk")
    ]
    
    var body: some View{
        List {
            ForEach(jokes) { joke in
                Text(joke.setup)
            }
        }
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
