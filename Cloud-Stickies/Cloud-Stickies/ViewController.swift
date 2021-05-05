//
//  ViewController.swift
//  Cloud-Stickies
//
//  Created by Alex Valente on 19/02/2021.
//

import Cocoa

class ViewController: NSViewController {
    
    @IBOutlet var textView: NSTextView!
    var windows: Array<NSWindow> = []
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        textView.backgroundColor = NSColor.green
        // Do any additional setup after loading the view.
    }

    override var representedObject: Any? {
        didSet {
        // Update the view, if already loaded.
        }
    }
    
    @IBAction func buttonOpen(_ sender: Any) {
        //let nib = NSNib(nibNamed: "Note", bundle: nil)
        //nib?.instantiate(withOwner: self, topLevelObjects: nil)
        
        //NoteController.createNewNote()
        //let win = NSWindow.init(contentViewController: bruh as NoteController)
        let win = NSWindow.init()
        win.styleMask.insert(.resizable)
        win.styleMask.insert(.closable)
        win.titlebarSeparatorStyle
        //win.styleMask.remove(.titled)
        win.setContentSize(NSSize(width: 300, height: 300))
        //win.minSize = CGSize(width: 300, height: 300)
        //win.maxSize = CGSize(width: 1000, height: 800)
        win.backgroundColor = NSColor.white
        win.makeKeyAndOrderFront(self)
        windows.append(win)
    }
}

