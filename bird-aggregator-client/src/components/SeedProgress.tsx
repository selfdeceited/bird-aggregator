import { Button, Popover } from "@blueprintjs/core"
import * as React from "react"
import styles from "../styles"
import * as hubBuilder from "../http.adapter";

interface ISeedProgressProps {

}

interface ISeedProgressState {
    allPhotos: number
    birdCount: number
    preloadedBirds: number
    preloadedPhotos: number
    stateString: string

    showComponent: boolean
    showBirdBar: boolean
    showPhotoBar: boolean
}

export class SeedProgress extends React.Component<ISeedProgressProps, ISeedProgressState> {
    constructor(props: ISeedProgressProps) {
        super(props)
        this.state = {
            allPhotos: 1,
            birdCount: 1,
            preloadedBirds: 0,
            preloadedPhotos: 0,
            showBirdBar: false,
            showComponent: true,
            showPhotoBar: false,
            stateString: "",
        }
        this.hide = this.hide.bind(this)
    }

    public componentDidMount() {
        const connection = hubBuilder.buildWithUrl("/seed");

        connection.start().then(() => {

            connection.invoke("Launch")
            connection.on("HidePopup", () => this.setState({ showComponent: false }))
            connection.on("Log", newLine =>
                this.setState({stateString: this.state.stateString + "\n " + newLine }))

            connection.on("QuantitySet", quantity => {
                this.setState({allPhotos: quantity, showPhotoBar: true})
                this.setState({stateString: this.state.stateString + "\n Photos required to load : " + quantity})
            })

            connection.on("BirdCount", birdCount => {
                this.setState({birdCount, showBirdBar: true})
                this.setState({stateString: this.state.stateString + "\n Birds required to load : " + birdCount})
            })

            connection.on("PhotoSaved", () => {
                this.setState({ preloadedPhotos: this.state.preloadedPhotos + 1 })

                if (this.state.preloadedPhotos === this.state.allPhotos && this.state.allPhotos !== 1) {
                    window.location.reload()
                }
            })

            connection.on("BirdSaved", () =>
                this.setState( { preloadedBirds: this.state.preloadedBirds + 1 }),
            )
        })
    }

    public render() {
        const progressBar = (caption: string, current: number, all: number) => {
            return (
                <div style={styles.barWrap}>
                <p>{caption} </p>
                    <div className={
                        "pt-progress-bar progress-bar " + (current === all
                         ? "pt-no-stripes pt-no-animation pt-intent-success"
                         : "pt-intent-primary")
                        }
                    >
                    <div className="pt-progress-meter"
                        style={{ width: `${current / all * 100 }%` }}></div>
                            <span>{current} / {all}</span>
                    </div>
                </div>)
        }
        const popoverButtonStyle = { margin: "10px", display: this.state.showComponent ? "block" : "none"}
        return (
            <div>
            {this.state.showComponent ? (
                <div className="seed-bar">
                    <h4>Application is starting...</h4>
                    { this.state.showBirdBar
                        ? progressBar("Birds", this.state.preloadedBirds, this.state.birdCount)
                        : null }

                    { this.state.showPhotoBar
                        ? progressBar("Photos", this.state.preloadedPhotos, this.state.allPhotos)
                        : null }
                    <Popover>
                        <Button text="What is happening?" style={popoverButtonStyle}/>
                        <div style={styles.seedPopover}>
                            Here's the part where I explain <s>why I'm such a shitty architect</s> what's happening.
                            <br/>
                            This site does not store or own any images.
                            <br/>
                            It just allows to view &amp; interact with photos located via external service (Flickr)
                            <br/>
                            Once this application is being launched first time,
                             it goes to Flickr to get all required metadata &amp; cache it.
                            <br/>
                            Since it's taking some time, it might confuse users,
                             it's better to be honest and show the actual progress.
                            <br/>
                            Once you reload the page, you can already navigate the site while pre-load
                             is being performed without seeing this info.
                        </div>
                    </Popover>
                    <div id="seed-log">{this.state.stateString}</div>
                    <Button text="Hide" onClick={this.hide} className="pt-button pt-minimal pt-small"/>
                </div>
                ) : null
            }
            </div>)
    }

    private hide() {
        this.setState(prevState => ({
            showComponent: false,
          }))
    }
}
