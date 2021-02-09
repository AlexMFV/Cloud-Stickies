//
//  CloudStickiesApp.swift
//  CloudStickies
//
//  Created by Alex Valente on 09/02/2021.
//

import SwiftUI

@main
struct CloudStickiesApp: App {
    let persistenceController = PersistenceController.shared

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environment(\.managedObjectContext, persistenceController.container.viewContext)
        }
    }
}
