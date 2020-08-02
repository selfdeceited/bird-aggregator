import * as Blueprint from "@blueprintjs/core"
import { Select, ItemPredicate, ItemRenderer  } from "@blueprintjs/labs"
import * as axios from "../http.adapter"
import * as React from "react"
import { Link } from "react-router-dom"

const tempBird = {id: 42, name: "some bird", latin: "some latin text"}
export type Bird = typeof tempBird

export interface INavbarProps { }
export interface INavbarState {
    flickr: string,
    github: string,
    owner: string,
    birds: Bird[],
    selectedBird: Bird
 }

const BirdSelect = Select.ofType<Bird>()

const filterBird: ItemPredicate<Bird> = (query: string, bird: Bird) => {
    return `${bird.name.toLowerCase()} ${bird.latin.toLowerCase()}`.indexOf(query.toLowerCase()) >= 0
}

const renderBird: ItemRenderer<Bird> = (bird: Bird, { handleClick, modifiers })=> (
    <Blueprint.MenuItem
        className=""
        key={bird.name}
        label={bird.latin}
        onClick={handleClick}
        text={bird.name}
    />
    )

export default class Navbar extends React.Component<INavbarProps, INavbarState>  {
    constructor(props: INavbarProps) {
        super(props)

        this.state = {
            birds: [],
            flickr: "",
            github: "",
            owner: "",
            selectedBird: { id: 0, name: "", latin: "" },
        }
    }

    public componentDidMount() {
        axios.get(`/api/links/flickr`).then(res => {
            const flickr = res.data
            this.setState({ flickr })
        })

        axios.get(`/api/links/github`).then(res => {
            const github = res.data
            this.setState({ github })
        })

        axios.get(`/api/links/user`).then(res => {
            const owner = res.data
            this.setState({ owner })
        })

        axios.get(`/api/birds/`).then(res => {
            const birds = res.data.birds
            this.setState({ birds })

            const selectedBird = birds[0]
            this.setState({ selectedBird })
        })
    }

    public render() {
        return  (
<nav className="bp3-navbar bp3-fixed-top">
    <div className="bp3-navbar-group bp3-align-left">
    <div className="logo"></div>
    <Link to="/" title="Main page" className="bp3-navbar-heading"><b>{this.state.owner}</b></Link>
    <span className="bp3-navbar-divider"></span>
    <Link to="/map" title="Map" role="button" className="bp3-button bp3-minimal bp3-icon-map small-margin">Map</Link>
    <Link to="/triplist" title="Trips" role="button"
        className="bp3-button bp3-minimal bp3-icon-torch small-margin">Trips</Link>
    <Link to="/lifelist" title="Life List" role="button"
        className="bp3-button bp3-minimal bp3-icon-numbered-list small-margin">Life List</Link>

    <span className="bp3-navbar-divider"></span>
    <div className="bird-info-select">
        <div className="inline-block">Find specific bird: &nbsp;</div>
        <BirdSelect
            items={this.state.birds}
            noResults={<Blueprint.MenuItem disabled text="No results." />}
            itemPredicate={filterBird}
            itemRenderer={renderBird}
            onItemSelect={this.onSelect}
        >
            <Blueprint.Button
                text={!!this.state.selectedBird ? this.state.selectedBird.name : ""}
                rightIcon="double-caret-vertical" />
        </BirdSelect>
        {
            !!this.state.selectedBird ?  (
            <span>
                <span className="small-space"></span>
                <Link to={"/birds/" + this.state.selectedBird.id}
                    role="button" className="bp3-button bp3-minimal bp3-icon-arrow-right"></Link>
            </span>) : null
        }
    </div>
  </div>
  <div className="bp3-navbar-group bp3-align-right">
    <a role="button" className="bp3-button bp3-minimal bp3-icon-git-repo" href={this.state.github}>GitHub</a>
    <a role="button" className="bp3-button bp3-minimal bp3-icon-group-objects" href={this.state.flickr}>Flickr</a>
  </div>
</nav>)
    }
    private onSelect = (bird: Bird) => {
        const selectedBird = bird
        this.setState({selectedBird})
    }
}
