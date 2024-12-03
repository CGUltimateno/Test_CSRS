module Main
open TicketManager
open SeatManager
open CinemaForm

type Program() =
    member this.Run() =
        let rows = 5
        let cols = 5
        let seatManager = SeatManager(rows, cols)
        let ticketManager = TicketManager()
        let showtimes = [| "10:00 AM"; "1:00 PM"; "4:00 PM"; "7:00 PM" |]
        let cinemaForm = CinemaForm(seatManager, ticketManager, showtimes)
        cinemaForm.Show()

// Entry point
[<EntryPoint>]
let main _ =
    let app = Program()
    app.Run()
    0